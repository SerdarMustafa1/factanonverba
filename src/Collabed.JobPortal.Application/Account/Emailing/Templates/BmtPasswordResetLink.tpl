<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional //EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" xmlns:o="urn:schemas-microsoft-com:office:office" xmlns:v="urn:schemas-microsoft-com:vml">
    <head>
        <meta content="text/html; charset=utf-8" http-equiv="Content-Type"/>
        <meta content="width=device-width" name="viewport"/>
        <meta content="IE=edge" http-equiv="X-UA-Compatible"/>
        <style type="text/css">
            body {
            margin: 0;
            padding: 0;
            }
            table,
            td,
            tr {
            vertical-align: top;
            border-collapse: collapse;
            }
            * {
            line-height: inherit;
            }
            a[x-apple-data-detectors=true] {
            color: inherit !important;
            text-decoration: none !important;
            }
        </style>
    </head>
    <body class="clean-body" style="margin: 0; padding: 0; -webkit-text-size-adjust: 100%;">
        <p>
        Hello,
        <br>
        We’ve received a request to reset the password for the <b>Build My Talent</b> account associated with <b>{{model.email}}</b>. No changes have been made to your account yet.
        <br>
        You can reset your password by clicking the link below:
        </p>

        <div>
        <a href="{{model.link}}">{{L "ResetYourPassword"}}</a>
        </div>

        <p>
        This link will expire after 24h of not being used.
        </p>
        <p>
        If you did not request a new password, please let us know immediately by sending an email to <a href="mailto:support@buildmytalent.com">support@buildmytalent.com</a>.
        <br>
        We are here to help you at any step along the way.
        <br>
        Sincerely,
        <br>
        The Build My Talent team.
        </p>
    </body>
</html>