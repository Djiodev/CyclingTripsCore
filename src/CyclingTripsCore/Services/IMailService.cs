﻿namespace CyclingTripsCore.Services
{
    public interface IMailService
    {
        void SendMail(string to, string from, string subject, string body);
    }
}