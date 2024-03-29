﻿using System.ComponentModel;
using LJX8000.Core.ViewModels.Controller;

namespace UI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Safely close all cameras
        /// </summary>
        /// <param name="e"></param>
        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);
            // Disconnect all controllers
            foreach (var controller in ControllerManager.AttachedControllers)
            {
                controller.IsConnectedHighSpeed = false;
            }
        }
    }
}