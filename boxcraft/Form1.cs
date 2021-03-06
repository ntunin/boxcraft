﻿using D3DX;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace boxcraft
{
    public partial class Form1 : D3DXForm
    {
        private Point center;
        public BoxCraftScene scene;

        public Form1()
        {
            InitializeForm();
            InitializeComponent();
            InitializeGraphics();
        }

        private void InitializeForm()
        {
            FormBorderStyle = FormBorderStyle.None;
            WindowState = FormWindowState.Maximized;
            MouseMove += new MouseEventHandler(OnMouseMove);
            MouseDown += new MouseEventHandler(OnMouseDown);
            KeyDown += new KeyEventHandler(OnKeyDown);
            KeyUp += new KeyEventHandler(OnKeyUp);
            Load += new EventHandler(OnLoad);
        }

        protected override Scene CreateScene()
        {
            scene = new BoxCraftScene(this);
            return scene;
        }

        private void OnLoad(object sender, EventArgs e)
        {
            center = new Point(Left + Width / 2, Top + Height / 2);
            Cursor.Position = center;
            Cursor.Hide();
        }

        private void OnMouseMove(object sender, MouseEventArgs e)
        {
            Point p = Cursor.Position;
            float deltaX = p.X - center.X;
            float deltaY = p.Y - center.Y;
            if (Math.Abs(deltaX) < 1.1 && Math.Abs(deltaY) < 1.1) {
                return;
            }
            deltaX = (float)(-deltaX * Math.PI / 180.0);
            deltaY = (float)(deltaY * Math.PI / 180.0);
            Cursor.Position = center;
            scene.Rotate(deltaX, deltaY);
        }

        private void OnMouseDown(object sender, MouseEventArgs e)
        {
            switch(e.Button)
            {
                case MouseButtons.Left:
                    scene.PerformRightHandAction();
                    break;
                case MouseButtons.Right:
                    scene.PerformLeftHandAction();
                    break;
            }
        }

        private void OnKeyDown(object sender, KeyEventArgs e)
        {
            switch(e.KeyCode)
            {
                case Keys.W:
                    scene.MoveToward(0);
                    break;
                case Keys.S:
                    scene.MoveToward((float)Math.PI);
                    break;
                case Keys.A:
                    scene.MoveToward((float)Math.PI/2);
                    break;
                case Keys.D:
                    scene.MoveToward(-(float)Math.PI/2);
                    break;
                case Keys.Space:
                    scene.MoveVertical(-1);
                    break;
                case Keys.ControlKey:
                    scene.MoveVertical(1);
                    break;
                case Keys.ShiftKey:
                    scene.speed = 10;
                    break;
                case Keys.D1:
                    scene.selectedBoxType = "ground";
                    break;
                case Keys.D2:
                    scene.selectedBoxType = "stone";
                    break;
                case Keys.D3:
                    scene.selectedBoxType = "bricks";
                    break;
                case Keys.D4:
                    scene.selectedBoxType = "wood";
                    break;
            }
        }

        private void OnKeyUp(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.ShiftKey:
                    scene.speed = 1;
                    break;
            }
        }
    }
}
