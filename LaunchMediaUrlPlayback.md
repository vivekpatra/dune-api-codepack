# launch\_media\_url: Playback #
## Summary ##

The following parameters are used to start playback and auto-detect media format.

## Syntax ##

| **Parameter**          | **Value(s)**       | **Example**                                     | **API Version(s)** |
|:-----------------------|:-------------------|:------------------------------------------------|:-------------------|
| [cmd](Cmd.md)            | launch\_media\_url | cmd=launch\_media\_url                          | ≥ 3                |
| [media\_url](MediaUrl.md) | {media\_url}       | media\_url=storage\_label://DuneHDD/example.mkv | ≥ 3                |
| [[timeout](Timeout.md)]  | {n}                | timeout=20                                      | ≥ 1                |

Most of the optional parameters used in [set\_playback\_state](SetPlaybackStateCommand.md) commands can also be used here.

## Examples ##

Start Blu-ray playback, auto-detect format:

`http://dune/cgi-bin/do?cmd=launch_media_url&media_url=storage_name://DuneHDD/Bluray/image.iso`


---


Start DVD playback, auto-detect format:

`http://dune/cgi-bin/do?cmd=launch_media_url&media_url=storage_name://DuneHDD/DVD/image.iso`


---


Start file playback, auto-detect format, start paused:

`http://dune/cgi-bin/do?cmd=launch_media_url&media_url=storage_name://DuneHDD/Converted/file.mkv&speed=0`


---


Start playlist playback, auto-detect format, skip the intro, hide on-screen display :

`http://dune/cgi-bin/do?cmd=launch_media_url&media_url=storage_name://DuneHDD/Music/album.m3u&start_index=1&hide_osd=1`


---


## Notes ##

This command does not return an error when the playback cannot be initialized. It is therefore better to use the more specific commands if the media format is known.