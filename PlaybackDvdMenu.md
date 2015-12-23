# playback\_dvd\_menu #
## Summary ##

`playback_dvd_menu` indicates whether a DVD menu is currently shown.

## Output ##

```
<?xml version="1.0" ?>
<command_result>
  ...
  <param name="playback_dvd_menu" value="{value}"/>
  ...
</command_result>
```

## Values ##

|             | **DVD Menu** |
|:------------|:-------------|
| **Visible**   | `1`          |
| **Invisible** | `0`          |

## Notes ##

This parameter breaks when DVD engine is set to type 2 (by always returning 0, even when it should be 1).