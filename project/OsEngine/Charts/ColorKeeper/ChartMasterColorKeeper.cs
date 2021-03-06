﻿/*
 *Ваши права на использование кода регулируются данной лицензией http://o-s-a.net/doc/license_simple_engine.pdf
*/

using System;
using System.Drawing;
using System.IO;
using System.Threading;
using OsEngine.Logging;

namespace OsEngine.Charts.ColorKeeper
{
    /// <summary>
    /// Хранилище цветов для чарта
    /// </summary>
    public class ChartMasterColorKeeper
    {

        /// <summary>
        /// имя
        /// </summary>
        private readonly string _name;

        /// <summary>
        /// конструктор
        /// </summary>
        /// <param name="name">имя панели, которой принадлежит</param>
        public ChartMasterColorKeeper(string name) 
        {
            _name = name;
            Load();
        }

        /// <summary>
        ///  загрузить из файла
        /// </summary>
        private void Load()
        {
            try
            {
                Thread.Sleep(500);

                if (!Directory.Exists(@"Engine\Color"))
                {
                    Directory.CreateDirectory(@"Engine\Color");
                }

                if (File.Exists(@"Engine\Color\" + _name + "Color.txt"))
                {
                    using (StreamReader reader = new StreamReader(@"Engine\Color\" + _name + "Color.txt"))
                    {
                        ColorUpBodyCandle = Color.FromArgb(Convert.ToInt32(reader.ReadLine()));
                        ColorUpBorderCandle = Color.FromArgb(Convert.ToInt32(reader.ReadLine()));
                        ColorDownBodyCandle = Color.FromArgb(Convert.ToInt32(reader.ReadLine()));
                        ColorDownBorderCandle = Color.FromArgb(Convert.ToInt32(reader.ReadLine()));
                        ColorBackSecond = Color.FromArgb(Convert.ToInt32(reader.ReadLine()));
                        ColorBackChart = Color.FromArgb(Convert.ToInt32(reader.ReadLine()));
                        ColorBackCursor = Color.FromArgb(Convert.ToInt32(reader.ReadLine()));
                        ColorText = Color.FromArgb(Convert.ToInt32(reader.ReadLine()));
                    }
                }
                else
                {
                    ColorUpBodyCandle = Color.DeepSkyBlue;
                    ColorUpBorderCandle = Color.Blue;

                    ColorDownBodyCandle = Color.DarkRed;
                    ColorDownBorderCandle = Color.Red;

                    ColorBackSecond = Color.Black;
                    ColorBackChart = Color.FromArgb(-15395563);
                    ColorBackCursor = Color.DarkOrange;

                    ColorText = Color.DimGray;
                }
            }
            catch (Exception error)
            {
                SendNewMessage(error.ToString(),LogMessageType.Error);
            }
        }

        /// <summary>
        /// сохранить в файл
        /// </summary>
        public void Save() 
        {
            try
            {
                using (StreamWriter writer = new StreamWriter(@"Engine\Color\" + _name + "Color.txt"))
                {
                    writer.WriteLine(ColorUpBodyCandle.ToArgb());
                    writer.WriteLine(ColorUpBorderCandle.ToArgb());
                    writer.WriteLine(ColorDownBodyCandle.ToArgb());
                    writer.WriteLine(ColorDownBorderCandle.ToArgb());

                    writer.WriteLine(ColorBackSecond.ToArgb());
                    writer.WriteLine(ColorBackChart.ToArgb());
                    writer.WriteLine(ColorBackCursor.ToArgb());
                    writer.WriteLine(ColorText.ToArgb());
                }

                if (NeedToRePaintFormEvent != null)
                {
                    NeedToRePaintFormEvent();
                }
            }
            catch (Exception error)
            {
                SendNewMessage(error.ToString(),LogMessageType.Error);
            }
        }

        /// <summary>
        /// удалить файл с настройками
        /// </summary>
        public void Delete()
        {
            try
            {
                if (File.Exists(@"Engine\Color\" + _name + "Color.txt"))
                {
                    File.Delete(@"Engine\Color\" + _name + "Color.txt");
                }
            }
            catch (Exception error)
            {
                SendNewMessage(error.ToString(),LogMessageType.Error);
            }
        }

        /// <summary>
        /// показать окно настроек
        /// </summary>
        public void ShowDialog() 
        {
            try
            {
                ChartMasterColorKeeperUi ui = new ChartMasterColorKeeperUi(this);
                ui.Show();
            }
            catch (Exception error)
            {
                SendNewMessage(error.ToString(),LogMessageType.Error);
            }
        }

        /// <summary>
        /// загрузить чёрную схему
        /// </summary>
        public void SetBlackScheme()
        {
            ColorUpBodyCandle = Color.DeepSkyBlue;
            ColorUpBorderCandle = Color.Blue;

            ColorDownBodyCandle = Color.DarkRed;
            ColorDownBorderCandle = Color.Red;

            ColorBackSecond = Color.Black;
            ColorBackChart = Color.FromArgb(255,27,27,27);
            ColorBackCursor = Color.DarkOrange;

            ColorText = Color.DimGray; 

            Save();

            if (NeedToRePaintFormEvent != null)
            {
                NeedToRePaintFormEvent();
            }
        }

        /// <summary>
        /// загрузить белую схему
        /// </summary>
        public void SetWhiteScheme()
        {
            ColorUpBodyCandle = Color.Azure;
            ColorUpBorderCandle = Color.Azure;

            ColorDownBodyCandle = Color.Black;
            ColorDownBorderCandle = Color.Black;

            ColorBackSecond = Color.Black;

            ColorBackChart = Color.FromArgb(255, 147, 147, 147);
            //ColorBackCursor = Color.DarkOrange;
            ColorBackCursor = Color.FromArgb(255, 255, 107, 0);

            ColorText = Color.Black;

            Save();

            if (NeedToRePaintFormEvent != null)
            {
                NeedToRePaintFormEvent();
            }
        }

 // цвета

        public Color ColorUpBodyCandle;

        public Color ColorDownBodyCandle;

        public Color ColorUpBorderCandle;

        public Color ColorDownBorderCandle;

        public Color ColorBackSecond;

        public Color ColorBackChart;

        public Color ColorBackCursor;

        public Color ColorText;

        /// <summary>
        /// событие изменения цвета в хранилище
        /// </summary>
        public event Action NeedToRePaintFormEvent;

        /// <summary>
        /// выслать наверх сообщение об ошибке
        /// </summary>
        private void SendNewMessage(string message, LogMessageType type)
        {
            if (LogMessageEvent != null)
            {
                LogMessageEvent(message, LogMessageType.Error);
            }
            else if (type == LogMessageType.Error)
            { // если никто на нас не подписан и происходит ошибка
                System.Windows.MessageBox.Show(message);
            }
        }

        /// <summary>
        /// исходящее сообщение для лога
        /// </summary>
        public event Action<string,LogMessageType> LogMessageEvent;

    }
}
