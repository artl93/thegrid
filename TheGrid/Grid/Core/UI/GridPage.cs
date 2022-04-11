using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;
using TheGrid.UI;
namespace TheGrid
{
    class GridPage : ControlPage
    {
        TrackGridFacade _model; 
        public const int Columns = 16;
        public const int Rows = 8;
        MusicalButton[,] _buttons;
        Texture2D _backgroundTexture;
        private Rectangle _backgroundLocation;
        private int _buttonSize;
        private Rectangle _viewSafeArea;
        public ButtonSelectBehavior SelectionBehavior;
        public int ColumnsPerQuarter {get; private set; }

        private int _activeColumn;

        public const int GridButtonHeight = 50;

        public override string Name
        {
            get { return _name; }
        }

        private GridPage() { }

        static public GridPage Create(string name, TrackGridFacade model, ButtonSelectBehavior buttonSelectBehavior, GetButtonColor function, int columnsPerQuarterNote)
        {
            var gridPage = new GridPage();
            gridPage._model = model;
            gridPage.ColumnsPerQuarter = columnsPerQuarterNote;
            gridPage.SelectionBehavior = buttonSelectBehavior;
            gridPage._GetButtonColor = function;
            gridPage._name = name;
            model.StartButtonAnimation += new EventHandler<ButtonStartEventArgs>(gridPage.model_StartButtonAnimation);
            return gridPage;
        }

        void model_StartButtonAnimation(object sender, ButtonStartEventArgs e)
        {
            _buttons[e.Column, e.Row].Play();
        }

        public override void Initialize(Rectangle viewSafeArea, Rectangle background, ContentManager Content)
        {
            _model.PreloadPatches();
            _backgroundLocation = background;
            _buttonSize = viewSafeArea.Width / Columns;
            _buttons = new MusicalButton[Columns, Rows];
            _viewSafeArea = viewSafeArea;

            int x = viewSafeArea.X;
            int y = viewSafeArea.Y;


            InitializeButtons(x, y);

            _backgroundTexture = Content.Load<Texture2D>("Background");
                        
        }

        private void InitializeButtons(int x, int y)
        {
            for (int col = 0; col < Columns; ++col)
            {
                for (int row = 0; row < Rows; ++row)
                {
                    var position = new Vector2(x + _buttonSize * col, y + _buttonSize * row);
                    _buttons[col, row] = CreateButton(col, row, position);
                }
            }
        }

        virtual protected MusicalButton CreateButton(int column, int row, Vector2 position)
        {
            return new MusicalButton(position, _GetButtonColor(row));
        }

        public delegate Color GetButtonColor(int row);
        GetButtonColor _GetButtonColor;
        private string _name;


        public override void Update(GameTime time)
        {
            base.Update(time);
            _activeColumn = _model.GetCurrentColumn() % Columns;
            
            foreach (var btn in _buttons)
                btn.IsActive = false;

            foreach (var btn in _model.GetActiveButtons())
                _buttons[btn.X, btn.Y].IsActive = true;

            for (int i = 0; i < Columns; ++i)
            {
                for (int j = 0; j < Rows; ++j)
                {
                    var button = _buttons[i, j];
                    button.IsOnBeat = i == _activeColumn;
                    button.Update(time);
                }
            }
        }

        override public void Draw(SpriteBatch batch)
        {
            batch.Draw(_backgroundTexture, _backgroundLocation, Color.White);

            foreach (var button in _buttons)
            {
                button.Draw(batch);
            }

            base.Draw(batch);
        }

        internal MusicalButton GetGridButtonAt(int x, int y)
        {
            var col = GetColumn(x);
            var row = GetRow(y);
            if (row < 0 || col < 0)
                return null;
            return _buttons[col, row];

        }

        internal Point? GetButtonLocation(int x, int y)
        {
            Point pt = new Point();
            var col = GetColumn(x);
            var row = GetRow(y);
            if (row < 0 || col < 0)
                return null;
            pt.X = col;
            pt.Y = row;
            return pt;
        }

        private int GetRow(int y)
        {
            var row = (y - _viewSafeArea.Y) / _buttonSize;
            if (row < 0 || row > _buttons.GetUpperBound(1))
                return -1;
            return row;
        }

        private int GetColumn(int x)
        {
            var col = (x - _viewSafeArea.X) / _buttonSize;
            if (col < 0 || col > _buttons.GetUpperBound(0))
                return -1;
            return col;
        }


        public override void HandleInput(Main.State currentState, Main.State previousState)
        {
            SelectionBehavior.HandleInput(this, _model, currentState, previousState);
            base.HandleInput(currentState, previousState);              

        }

        private bool IsPointInSafeViewArea(int x, int y)
        {
            return (x >= 0 && _viewSafeArea.X < x) && (y >= 0 && _viewSafeArea.X < y);
        }

        public int GridResolution { get; set; }

        internal void Clear()
        {
            _model.Clear();
        }

        internal void ClearColumn(int x)
        {
            var col = GetColumn(x);
            if (col < 0)
                return;
            for (int i = 0; i <= _buttons.GetUpperBound(1); ++i)
            {
                _buttons[col, i].IsActive = false;
            }
            _model.ClearEventsAtColumn(col, Rows);
        }
    }
}
