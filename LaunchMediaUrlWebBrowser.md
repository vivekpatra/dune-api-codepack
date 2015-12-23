# launch\_media\_url: Web Browser #
## Summary ##

The following parameters can be used to open a webpage in the web browser.

## Syntax ##

| **Parameter**          | **Value(s)**        | **Example**                          | **API Version(s)** |
|:-----------------------|:--------------------|:-------------------------------------|:-------------------|
| [cmd](Cmd.md)            | launch\_media\_url  | cmd=launch\_media\_url               | ≥ 3                |
| [media\_url](MediaUrl.md) | www://{media\_url}  | media\_url=www://http://dune-hd.com  | ≥ 3                |
| [[timeout](Timeout.md)]  | {n}                 | timeout=20                           | ≥ 1                |

Optional parameters are appended to the media\_url, separated by tripple colons (`:::`).

## Optional parameters ##

The web browser takes the following optional parameters:

| **Parameter**          | **Value(s)**                               | **Default**       | **Summary**                                                        |
|:-----------------------|:-------------------------------------------|:------------------|:-------------------------------------------------------------------|
| fullscreen             | 0 | 1                                      | 0                 | Indicates whether to hide browser controls.                        |
| webapp\_keys           | 0 | 1                                      | 0                 | Indicates whether all key input should be handled by JavaScript.   |
| zoom\_level            | {n}                                        | system setting    | Indicates the zoom level percentage.                               |
| overscan               | {n}                                        | system setting    | Indicates the overscan percentage.                                 |
| user\_agent            | {text}                                     | browser defined   | Indicates the user agent used in HTTP requests.                    |
| background\_color      | white | black                              | white             | Indicates the background color used by the browser.                |
| osd\_size              | 720x480 | 720x576 | 1280x720 | 1920x1080   | current mode      | Indicates the video mode.                                          |
| input\_handler         | ?                                          | ?                 | ?                                                                  |
| navigation\_highlight  | 0 | 1                                      | 1                 | ?                                                                  |
| exit\_key              | ?                                          | ?                 | ?                                                                  |
| power\_key             | ?                                          | ?                 | ?                                                                  |

They must be appended to the media\_url by separating the URL and parameters with triple colons (`:::`).

## Examples ##

Launch dune-hd.com in the webbrowser, hide the browser controls, set background color to white:

`http://dune/cgi-bin/do?cmd=launch_media_url&media_url=www://http://dune-hd.com/:::fullscreen=1&background_color=white`