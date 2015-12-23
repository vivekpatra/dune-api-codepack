# cmd #
## Summary ##

`cmd` indicates an API method.

## Values ##

| **Command**                                              | **Summary**                                                        | **API Version(s)** |
|:---------------------------------------------------------|:-------------------------------------------------------------------|:-------------------|
| [status](StatusCommand.md)                                 | Gets the device status without affecting it in any way.            | ≥ 1                |
| [main\_screen](MainScreenCommand.md)                        | Sets the device to the start-up screen.                            | ≥ 1                |
| [black\_screen](BlackScreenCommand.md)                      | Sets the device to black screen mode.                              | ≥ 1                |
| [standby](StandbyCommand.md)                               | Sets the device to standby mode.                                   | ≥ 1                |
| [ir\_code](IrCodeCommand.md)                                | Emulates a remote control button press.                            | ≥ 1                |
| [dvd\_navigation](DvdNavigationCommand.md)                  | Navigates through a DVD menu.                                      | ≥ 1                |
| [bluray\_navigation](BlurayNavigationCommand.md)            | Navigates through a Blu-ray menu.                                  | ≥ 1                |
| [set\_playback\_state](SetPlaybackStateCommand.md)           | Manipulates various playback settings.                             | ≥ 1                |
| [start\_dvd\_playback](StartDvdPlaybackCommand.md)           | Starts DVD playback from a DVD file or folder structure.           | ≥ 1                |
| [start\_bluray\_playback](StartBlurayPlaybackCommand.md)     | Starts Blu-ray playback from a Blu-ray file or folder structure.   | ≥ 1                |
| [start\_file\_playback](StartFilePlaybackCommand.md)         | Starts file playback from an arbitrary file or file stream.        | ≥ 1                |
| [start\_playlist\_playback](StartPlaylistPlaybackCommand.md) | Starts playlist playback from a playlist file or folder.           | ≥ 3                |
| [launch\_media\_url](LaunchMediaUrlCommand.md)               | Multiple uses.                                                     | ≥ 3                |
| [get\_text](GetTextCommand.md)                              | When on a text input field, gets the current text value.           | ≥ 3                |
| [set\_text](SetTextCommand.md)                              | When on a text input field, changes it to the specified value.     | ≥ 3                |
| [light\_stop](LightStop.md)                                 | Exits playback mode and sets the device to the previous view.      | ≥ 3 (b6)           |
| [open\_path](OpenPath.md)                                   | Navigates to a folder menu.                                        | ≥ 3 (b6)           |
| [start\_dvbs\_scan](StartDvbsScan.md)                        |                                                                    | ≥ 3 (b8)           |
| [start\_dvbc\_scan](StartDvbcScan.md)                        |                                                                    | ≥ 3 (b8)           |
| [cancel\_dvb\_scan](CancelDvbScan.md)                        |                                                                    | ≥ 3 (b8)           |

## Notes ##

`cmd` is the only parameter that is required for every command. Depending on its value, additional parameters may be required.