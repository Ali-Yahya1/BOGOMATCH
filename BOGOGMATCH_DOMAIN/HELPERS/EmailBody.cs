﻿namespace BOGOMATCH.Helpers
{
    public static class EmailBody
    {
        public static string EmailStringbody(string email, string emailToken)
        {
            return $@"
                <html>
                <head></head>
                <body style=""margin:0;padding:0;font-family: Arial, Helvetica, sans-serif;"">
                    <div style=""height: auto; background: linear-gradient(to top, #c9c9ff 50%, #6e6ef6 90%) no-repeat; width: 400px; padding:30px;"">
                        <div>
                            <h1>Reset your Password</h1>
                            <hr>
                            <p>You're receiving this e-mail because you requested a password reset for your BOGOMATCH account.</p>
                            <p>Please tap the button below to choose a new password.</p>
                            <a href=""http://localhost:4200/reset-password?email={email}&code={emailToken}""
                               target=""_blank""
                               style=""background: #0d6efd; color: white; border-radius: 4px; display:block; margin: 0 auto; width: 50%; text-align: center; padding: 10px 0; text-decoration: none;"">
                                Reset Password
                            </a>
                            <p>Kind Regards,<br><br>
                            BOGOMATCH TEAM</p>
                        </div>
                    </div>
                </body>
                </html>";
        }
    }
}
