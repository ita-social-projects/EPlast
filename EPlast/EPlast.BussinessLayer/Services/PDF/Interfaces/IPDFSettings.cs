﻿namespace EPlast.BussinessLayer
{
    public interface IPDFSettings
    {
        string Title { get; set; }
        string Subject { get; set; }
        string Author { get; set; }
        string FontName { get; set; }
        string StyleName { get; set; }
        string ImagePath { get; set; }
    }
}