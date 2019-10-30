﻿namespace LJX8000.Core.IoC.Interface
{
    public interface IUILogger
    {
        void LogThreadSafe(string message);
    }
}