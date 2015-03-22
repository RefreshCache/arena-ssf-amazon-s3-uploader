This is a module developed by Joe Adams at Southside Fellowship. It provides a simple way for administrators to configure and control who has access to their Amazon AWS account, by only allowing users to upload to certain paths in their buckets on S3.

Our organization has several people who need to be able to upload various files (media, podcasts, pdfs, etc.) to our S3 hosting area, but we didn't want to give everyone the master login or give out access keys where the (potentially) inexperienced, untrained users might put their files in the wrong bucket or folder. This module has been designed to simplify the process for them.

**Features:**
* Specify the access and secret keys to Amazon AWS in the module settings, and with Arena's built-in security, administrators can give access to an instance of a module without having to give users the actual login or keys (Simplifies the process for them)
* Specify the bucket and folder path, so the user does not have to choose or remember
* Specify the default security for files uploaded within that module (PublicRead, PublicReadWrite, AuthenticatedRead, Private)
* Send custom email report to an email address (default value can be provided in module settings) with information about file(s) uploaded

The email features were something we needed here. If the email destination setting is left blank in the module settings, it will ignore all email functions. Multiple files can be uploaded at a time, and only one email will be generated once the user is ready to send it.

**This project has two dependencies:**
This module requires the Flajaxian core upload DLL and its DirectToAmazon DLL. This allows the module to upload straight from the client to Amazon, bypassing the server, which saves bandwidth and processing time. The DirectToAmazon DLL had to be modified from its original source code in order to be compatible with Arena (there was a function that was inadvertently stripping off some of the querystring values in the POST, which was keeping Arena from serving it correctly).
