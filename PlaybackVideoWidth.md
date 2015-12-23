# playback\_video\_width #
## Summary ##

`playback_video_width` indicates the current video stream's width.

## Output ##

```
<?xml version="1.0" ?>
<command_result>
  ...
  <param name="playback_video_width" value="{n}"/>
  ...
</command_result>
```

## Values ##

Actual value depends on the video stream.

|           | **Width**       |
|:----------|:----------------|
| **Minimum** | `0`             |
| **Maximum** | `{osd_width}`   |