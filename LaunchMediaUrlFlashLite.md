# launch\_media\_url: Flash Lite #
## Summary ##

The following parameters can be used to launch a Flash Lite application.

## Syntax ##

| **Parameter**          | **Value(s)**        | **Example**                                    | **API Version(s)** |
|:-----------------------|:--------------------|:-----------------------------------------------|:-------------------|
| [cmd](Cmd.md)            | launch\_media\_url  | cmd=launch\_media\_url                         | ≥ 3                |
| [media\_url](MediaUrl.md) | swf://{media\_url}  | media\_url=swf://http://example.com/flash.swf  | ≥ 3                |
| [[timeout](Timeout.md)]  | {n}                 | timeout=20                                     | ≥ 1                |

Optional FlashVars are appended to the media\_url, separated by triple colons (`:::`).

## Optional parameters ##

If a Flash Lite application requires a set of FlashVars, they must be appended to the media\_url by separating the URL and parameters with triple colons(`:::`).

## Examples ##

Launch a custom Flash Lite app that takes two FlashVar (var1 and var2):

`http://dune/cgi-bin/do?cmd=launch_media_url&media_url=swf://http://example.com/flashapp.swf:::var1=hello&var2=world`