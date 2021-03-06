﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace MedLab.Classes
{
    public class CaptchaGen
    {
        private static char[] captchaSymbols = "0123456789qwertyuiopasdfghjklzxcvbnm".ToCharArray();
        private static List<SolidColorBrush> brushes = new List<SolidColorBrush>()
        {
            Brushes.Magenta,
            Brushes.Blue,
            Brushes.Red
        };

        public static string GenerateCaptcha(Canvas canvas)
        {
            Random random = new Random();
            string captcha = "";
            
            canvas.Background = Brushes.White;

            for (int i = 0; i < 600; i++)
            {
                Ellipse ellipse = new Ellipse
                {
                    Fill = brushes[random.Next(0, brushes.Count())],
                    Height = random.NextDouble()*10,
                    Width = random.NextDouble()*10,
                    Margin = new Thickness(random.NextDouble()*240, random.NextDouble()*70,0,0),
                    Visibility = Visibility.Visible
                };

                canvas.Children.Add(ellipse);
            }

            for (int i = 0; i < 6; i++)
            {
                captcha += captchaSymbols[random.Next(0, captchaSymbols.Length)].ToString();
            }

            Label label = new Label()
            {
                Content = captcha,
                Margin = new Thickness(120, 30, 0, 0),
                FontSize = 24,
                FontWeight = FontWeights.Bold,
                RenderTransform = new RotateTransform(random.NextDouble()*10)
            };

            canvas.Children.Add(label);

            return captcha;
        }
    }
}
