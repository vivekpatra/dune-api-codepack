# playback\_url #
## Summary ##

`playback_url` indicates the media URL that is currently playing.

## Output ##

```
<?xml version="1.0" ?>
<command_result>
  ...
  <param name="playback_url" value="{value}"/>
  ...
</command_result>
```

## Values ##

Media URLs in one of the supported formats ([info](MediaUrl.md)).

## Notes ##

When playback is initiated using the player navigator, the value will not be a valid media\_url. Instead it will be a unix filepath using one of the following formats:

`/D/...` for locally attached storage.

`/tmp/mnt/smb/....` for files on SMB mounts.

`/tmp/mnt/nfs/....` for files on NFS mounts.

`/tmp/mnt/network/...` for files on named network shares.