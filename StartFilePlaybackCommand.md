# start\_file\_playback #
## Summary ##

`start_file_playback` commands start playback sessions from all media URLs which do not point to playlist, DVD or Blu-ray structures.

Use cases also include URLs pointing to a live stream, such as internet radio or IPTV streams, and DLNA streams.

## Syntax ##

| **Parameter**          | **Value(s)**          | **Example**                                   | **API Version(s)** |
|:-----------------------|:----------------------|:----------------------------------------------|:-------------------|
| [cmd](Cmd.md)            | start\_file\_playback | cmd=start\_file\_playback                     | ≥ 1                |
| [media\_url](MediaUrl.md) | {media\_url}          | media\_url=storage\_label://DuneHDD/movie.mkv | ≥ 1                |
| [[timeout](Timeout.md)]  | {n}                   | timeout=20                                    | ≥ 1                |

Most of the optional parameters used in [set\_playback\_state](SetPlaybackStateCommand.md) commands can also be used here.

## Examples ##

Start file playback from an MP3 file, stored on the internal hard drive:

`http://dune/cgi-bin/do?cmd=start_file_playback&media_url=storage_label://DuneHDD/example.mp3`


---


Start file playback from an MP3 file stored in a web location:

`http://dune/cgi-bin/do?cmd=start_file_playback&media_url=http://example.com/example.mp3`


---


Start file playback from a MKV file, stored on an SMB server on the local network. Start with paused playback:

`http://dune/cgi-bin/do?cmd=start_file_playback&media_url=smb://host/path/example.mkv&speed=0`


---


Start file playback from an MKV file, stored on the internal hard drive. Start at the 10 minute mark, restart playback when finished:

`http://dune/cgi-bin/do?cmd=start_file_playback&media_url=storage_label://DuneHDD/example.mkv&position=600&action_on_finish=restart_playback`


---


## Notes ##

  * When working with locally attached storage, always try to use the `storage_name:// | storage_uuid:// | storage_label://` schemes rather than the `smb://` scheme.
  * Starting with API version 3, [launch\_media\_url](http://code.google.com/p/dune-api-codepack/wiki/LaunchMediaUrlCommand) can be used to automatically detect the media type.
