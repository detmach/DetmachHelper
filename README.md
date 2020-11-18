## DetmachHelper

### Kişisel bir kaç yardımcı kod.


    var canonicalUrlOptions = new CanonicalURLMiddlewareOptions();
    canonicalUrlOptions.EnforceLowerCaseUrls = true;
    canonicalUrlOptions.RemoveTrailingSlash = true;
    canonicalUrlOptions.QueryStringCaseSensitive = true;
    app.UseCanonicalUrlMiddleware(canonicalUrlOptions)


