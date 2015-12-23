# start\_playlist\_playback #
## Summary ##

`start_playlist_playback` commands start playback sessions from media URLs which point to a playlist file or folder structure.

Use cases also include internet radio streams where internet streams are grouped in a playlist structure.

## Syntax ##

| **Parameter**                | **Value(s)**              | **Example**                                     | **API Version(s)** |
|:-----------------------------|:--------------------------|:------------------------------------------------|:-------------------|
| [cmd](Cmd.md)                  | start\_playlist\_playback | cmd=start\_playlist\_playback                   | ≥ 3                |
| [media\_url](MediaUrl.md)       | {media\_url}              | media\_url=storage\_label://DuneHDD/example.m3u | ≥ 3                |
| [[start\_index](StartIndex.md)] | {n}                       | start\_index=3                                  | ≥ 3                |
| [[timeout](Timeout.md)]        | {n}                       | timeout=20                                      | ≥ 1                |


## Examples ##

Start playlist playback from a folder on the internal hard drive:

`http://dune/cgi-bin/do?cmd=start_playlist_playback&media_url=storage_name://DuneHDD_abcd_1234_efgh/music/album/`


---


Start playlist playback from a playlist file stored on the internal hard drive. Skip the first file, and show nothing on the screen:

`http://dune/cgi-bin/do?cmd=start_playlist_playback&media_url=storage_name://DuneHDD_abcd_1234_efgh/playlist.m3u&start_index=1&black_screen=1`


---


## Notes ##

  * When working with locally attached storage, always try to use the `storage_name:// | storage_uuid:// | storage_label://` schemes rather than the `smb://` scheme.
  * Starting with API version 3, [launch\_media\_url](http://code.google.com/p/dune-api-codepack/wiki/LaunchMediaUrlCommand) can be used to automatically detect the media type.
