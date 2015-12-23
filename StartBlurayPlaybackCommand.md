# start\_bluray\_playback #
## Summary ##

`start_bluray_playback` commands start playback sessions from all media URLs which point to a Blu-ray file or folder structure.

## Syntax ##

| **Parameter**          | **Value(s)**            | **Example**                                     | **API Version(s)** |
|:-----------------------|:------------------------|:------------------------------------------------|:-------------------|
| [cmd](Cmd.md)            | start\_bluray\_playback | cmd=start\_bluray\_playback                     | ≥ 1                |
| [media\_url](MediaUrl.md) | {media\_url}            | media\_url=storage\_label://DuneHDD/example.iso | ≥ 1                |
| [[timeout](Timeout.md)]  | {n}                     | timeout=20                                      | ≥ 1                |

Most of the optional parameters used in [set\_playback\_state](SetPlaybackStateCommand.md) commands can also be used here.

## Examples ##

Start Blu-ray playback from a Blu-ray disc (on supported models):

`http://dune/cgi-bin/do?cmd=start_bluray_playback&media_url=storage_name://optical_drive/`


---


Start Blu-ray playback from a Blu-ray folder structure, stored on the internal hard drive. Start paused at the 3 minute mark:

`http://dune/cgi-bin/do?cmd=start_bluray_playback&media_url=storage_label://DuneHDD/example/BDMV/&speed=0&position=180`


---


Start Blu-ray playback from a Blu-ray folder structure, stored on an NFS server on the local network.

`http://dune/cgi-bin/do?cmd=start_bluray_playback&media_url=nfs://server:/export-path:/example/BDMV/`


---


Start Blu-ray playback from a Blu-ray image stored on an SMB server on the local network:

`http://dune/cgi-bin/do?cmd=start_bluray_playback&media_url=smb://server/path/example.iso`


---


## Notes ##

  * When working with locally attached storage, always try to use the `storage_name:// | storage_uuid:// | storage_label://` schemes rather than the `smb://` scheme.
  * Commands involving `optical_drive` fail most of the time, depending on the disc used.
  * Starting with API version 3, [launch\_media\_url](http://code.google.com/p/dune-api-codepack/wiki/LaunchMediaUrlCommand) can be used to automatically detect the media type.
