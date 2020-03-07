// action：
// file name：${namespace}.ImageExt.cs
// author：lynx lynx.kor@163.com @ 2019/6/6 17:56
// copyright (c) 2019 lynxce.com
// desc：
// > add description for ImageExt
// revision：
//
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;

namespace lce.provider
{
    public static class ImageExt
    {
        /// <summary>
        /// 生成Code对应的验证码图片
        /// </summary>
        /// <returns>The captcha.</returns>
        /// <param name="code">Code.</param>
        public static MemoryStream Captcha(string code)
        {
            Random random = new Random();
            //验证码颜色集合  
            Color[] c = { Color.Black, Color.Red, Color.DarkBlue, Color.Green, Color.Orange, Color.Brown, Color.DarkCyan, Color.Purple };
            //验证码字体集合
            string[] fonts = { "Verdana", "Microsoft Sans Serif", "Comic Sans MS", "Arial", "宋体" };
            //定义图像的大小，生成图像的实例  
            var img = new Bitmap(code.Length * 16, 32);
            var g = Graphics.FromImage(img);
            g.Clear(Color.White);//背景设为白色  
            //在随机位置画背景点  
            for (int i = 0; i < 100; i++)
            {
                int x = random.Next(img.Width);
                int y = random.Next(img.Height);
                g.DrawRectangle(new Pen(Color.LightGray, 0), x, y, 1, 1);
            }
            //验证码绘制在g中 
            for (int i = 0; i < code.Length; i++)
            {
                int cindex = random.Next(7);//随机颜色索引值  
                int findex = random.Next(5);//随机字体索引值  
                Font f = new Font(fonts[findex], 15, FontStyle.Bold);//字体  
                Brush b = new SolidBrush(c[cindex]);//颜色  
                int ii = 4;
                if ((i + 1) % 2 == 0)//控制验证码不在同一高度  
                {
                    ii = 2;
                }
                g.DrawString(code.Substring(i, 1), f, b, 3 + (i * 12), ii);//绘制一个验证字符  
            }
            var ms = new MemoryStream();
            img.Save(ms, ImageFormat.Png);//将此图像以Png图像文件的格式保存到流中  
            //回收资源  
            g.Dispose();
            img.Dispose();
            return ms;
        }

        /// <summary>
        /// Thumbnail the specified source, target, size and side.
        /// </summary>
        /// <returns>The thumbnail.</returns>
        /// <param name="source">Source.</param>
        /// <param name="target">Target.存储文件名，后缀.png，并且只能有一个(.)这应该是一个基类里的bug</param>
        /// <param name="size">Size.</param>
        /// <param name="side">If set to <c>true</c> side.</param>
        public static bool Thumbnail(this Image source, string target, int size = 800, bool side = true)
        {
            // 等比例尺寸计算
            var sSize = new Size(source.Width, source.Height);
            int tw;
            int th;
            if ((side && sSize.Width < size) || (!side && sSize.Height < size))
            {
                tw = sSize.Width;
                th = sSize.Height;
            }
            else
            {
                if (side)
                {
                    tw = size;
                    th = sSize.Height * size / sSize.Width;
                }
                else
                {
                    th = size;
                    tw = sSize.Width * size / sSize.Height;
                }
            }
            return source.Thumbnail(target, size, size, tw, th, false);
        }


        /// <summary>
        /// Thumbnail the specified source, target, width, height and quality.
        /// </summary>
        /// <returns>The thumbnail.</returns>
        /// <param name="source">Source.</param>
        /// <param name="target">Target.存储文件名，后缀.png，并且只能有一个(.)这应该是一个基类里的bug</param>
        /// <param name="width">Width.</param>
        /// <param name="height">Height.</param>
        /// <param name="isFixed">是否比例换算后绘制到固定width,height画布中,true：是；false；画布也等比例</param>
        public static bool Thumbnail(this Image source, string target, int width = 800, int height = 800, bool isFixed = true)
        {
            // 等比例尺寸计算
            var sSize = new Size(source.Width, source.Height);
            int tw;
            int th;
            if (sSize.Width < width && sSize.Height < height)
            {
                tw = sSize.Width;
                th = sSize.Height;
            }
            else
            {
                if ((sSize.Width / width) > (sSize.Height / height))
                {
                    tw = width;
                    th = sSize.Height * width / sSize.Width;
                }
                else
                {
                    th = height;
                    tw = sSize.Width * height / sSize.Height;
                }
            }
            return source.Thumbnail(target, width, height, tw, th, isFixed);
        }

        /// <summary>
        /// Thumbnail the specified source, target, sWidth, sHeight, tWidth, tHeight and isFixed.
        /// </summary>
        /// <returns>The thumbnail.</returns>
        /// <param name="source">Source.</param>
        /// <param name="target">Target.</param>
        /// <param name="sWidth">S width.</param>
        /// <param name="sHeight">S height.</param>
        /// <param name="tWidth">T width.</param>
        /// <param name="tHeight">T height.</param>
        /// <param name="isFixed">If set to <c>true</c> is fixed.</param>
        public static bool Thumbnail(this Image source, string target, int sWidth, int sHeight, int tWidth, int tHeight, bool isFixed = true)
        {
            var bitmap = new Bitmap(sWidth, sHeight);
            if (!isFixed) bitmap = new Bitmap(tWidth, tHeight);// 不固定画布
            var g = Graphics.FromImage(bitmap);
            g.Clear(Color.Transparent);
            g.CompositingQuality = CompositingQuality.HighQuality;
            g.SmoothingMode = SmoothingMode.HighQuality;
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;
            if (isFixed)
                g.DrawImage(source, new Rectangle((sWidth - tWidth) / 2, (sHeight - tHeight) / 2, tWidth, tHeight), 0, 0, source.Width, source.Height, GraphicsUnit.Pixel);
            else
                g.DrawImage(source, new Rectangle(0, 0, tWidth, tHeight), 0, 0, source.Width, source.Height, GraphicsUnit.Pixel);

            g.Dispose();
            try
            {
                bitmap.Save(target, ImageFormat.Png);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message, ex);
                return false;
            }
            finally
            {
                source.Dispose();
                bitmap.Dispose();
            }
        }
    }
}
