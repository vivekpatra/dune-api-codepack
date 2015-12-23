# playback\_is\_buffering #
## Summary ##

`playback_is_buffering` indicates whether the playback is buffering.

## Output ##

```
<?xml version="1.0" ?>
<command_result>
  ...
  <param name="playback_is_buffering" value="{value}"/>
  ...
</command_result>
```

## Values ##

|         | **Buffering** |
|:--------|:--------------|
| **True**  | `1`           |
| **False** | `0`           |

## Notes ##

The introduction of the [playback\_state](PlaybackState.md) parameter in protocol version 3 effectively renders this parameter obsolete.