# subtitles\_track #
## Summary ##

`subtitles_track` indicates the active subtitles track.

## Input ##

| **Parameter**        | **Value(s)**         |
|:---------------------|:---------------------|
| cmd                  | set\_playback\_state |
| subtitles\_track     | {n}                  |
| [[timeout](timeout.md)]        | {n}                  |

## Output ##

```
<?xml version="1.0" ?>
<command_result>
  ...
  <param name="subtitles_track" value="{n}"/>
  ...
</command_result>
```

## Values ##

|            | **Track**                         |
|:-----------|:----------------------------------|
| **Minimum**  | `0`                               |
| **Maximum**  | `{sum of subtitles tracks} - 1`   |
| **Default**  | `0*`                              |

`*`: the preferred language setting (if set) takes precedence over the default value.

## Notes ##

In order to disable subtitles, set the value to a number greater than 63.