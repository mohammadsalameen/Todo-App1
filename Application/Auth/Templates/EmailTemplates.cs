namespace Todo_App.Application.Auth.Templates
{
    public static class EmailTemplates
    {
        public static string ConfirmEmail(string userName, string confirmUrl)
        {
            return $@"
         <!DOCTYPE html>
         <html>
         <head>
         <meta charset='UTF-8'>
                           <style>
                            body {{
                                font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif;
                                 background-color: #eef2f7;
                                  padding: 20px;
        }}
        .card {{
            max-width: 600px;
            margin: auto;
            background: #ffffff;
            border-radius: 12px;
            padding: 40px 30px;
            box-shadow: 0 8px 20px rgba(0,0,0,0.1);
        }}
        h2 {{
            color: #1f2937;
            font-size: 24px;
        }}
        p {{
            color: #4b5563;
            line-height: 1.6;
            font-size: 16px;
        }}
        .btn {{
            display: inline-block;
            margin-top: 25px;
            padding: 14px 28px;
            background: linear-gradient(90deg, #4f46e5, #3b82f6);
            color: #ffffff !important;
            text-decoration: none;
            border-radius: 8px;
            font-weight: bold;
            transition: all 0.3s ease;
        }}
        .btn:hover {{
            background: linear-gradient(90deg, #4338ca, #2563eb);
        }}
        .footer {{
            margin-top: 35px;
            font-size: 12px;
            color: #9ca3af;
            text-align: center;
        }}
        a {{
            color: #2563eb;
        }}
    </style>
</head>
<body>
    <div class='card'>
        <h2>Hello, {userName} 👋</h2>
        <p>
            Thank you for signing up for <b>Todo App</b>. 
            Please confirm your email address by clicking the button below.
        </p>

        <a href='{confirmUrl}' class='btn'>Confirm Email</a>

        <p style='margin-top:20px;'>
            If the button doesn’t work, copy and paste this link into your browser:
        </p>

        <p>
            <a href='{confirmUrl}'>{confirmUrl}</a>
        </p>

        <div class='footer'>
            © {DateTime.Now.Year} Todo App. All rights reserved.
        </div>
    </div>
</body>
</html>";
        }
        public static string SendCodeEmail(string userName, string code)
        {
            return $@"
<!DOCTYPE html>
<html>
<head>
    <meta charset='UTF-8'>
    <style>
        body {{
            font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif;
            background-color: #eef2f7;
            padding: 20px;
        }}
        .card {{
            max-width: 600px;
            margin: auto;
            background: #ffffff;
            border-radius: 12px;
            padding: 40px 30px;
            box-shadow: 0 8px 20px rgba(0,0,0,0.1);
            text-align: center;
        }}
        h2 {{
            color: #1f2937;
            font-size: 24px;
        }}
        p {{
            color: #4b5563;
            line-height: 1.6;
            font-size: 16px;
        }}
        .code {{
            margin: 30px 0;
            font-size: 32px;
            font-weight: bold;
            letter-spacing: 6px;
            color: #4f46e5;
            background: #f3f4f6;
            padding: 16px;
            border-radius: 10px;
            display: inline-block;
        }}
        .footer {{
            margin-top: 35px;
            font-size: 12px;
            color: #9ca3af;
            text-align: center;
        }}
    </style>
</head>
<body>
    <div class='card'>
        <h2>Hello, {userName} 👋</h2>

        <p>
            We received a request to verify your identity.<br/>
            Please use the verification code below:
        </p>

        <div class='code'>{code}</div>

        <p>
            This code is valid for a limited time.<br/>
            If you didn’t request this, you can safely ignore this email.
        </p>

        <div class='footer'>
            © {DateTime.Now.Year} Todo App. All rights reserved.
        </div>
    </div>
</body>
</html>";
        }


        public static string ChangePasswordEmail(string userName)
        {
            return $@"
<!DOCTYPE html>
<html>
<head>
    <meta charset='UTF-8'>
    <style>
        body {{
            font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif;
            background-color: #eef2f7;
            padding: 20px;
        }}
        .card {{
            max-width: 600px;
            margin: auto;
            background: #ffffff;
            border-radius: 12px;
            padding: 40px 30px;
            box-shadow: 0 8px 20px rgba(0,0,0,0.1);
        }}
        h2 {{
            color: #1f2937;
            font-size: 24px;
        }}
        p {{
            color: #4b5563;
            line-height: 1.7;
            font-size: 16px;
        }}
        .success {{
            background-color: #ecfdf5;
            color: #065f46;
            padding: 15px;
            border-radius: 8px;
            margin-top: 25px;
            border-left: 4px solid #10b981;
            font-size: 14px;
        }}
        .footer {{
            margin-top: 40px;
            font-size: 12px;
            color: #9ca3af;
            text-align: center;
        }}
    </style>
</head>
<body>
    <div class='card'>
        <h2>Password Changed Successfully ✅</h2>

        <p>Hello <b>{userName}</b>,</p>

        <p>
            This email confirms that your Todo App account password has been
            changed successfully.
        </p>

        <div class='success'>
            ✔️ If this was you, no further action is required.
            <br />
            ❗ If you did not make this change, please reset your password immediately.
        </div>

        <div class='footer'>
            © {DateTime.Now.Year} Todo App. All rights reserved.
        </div>
    </div>
</body>
</html>";
        }

    }
}
