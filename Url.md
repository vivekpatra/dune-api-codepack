# url #
## Summary ##

`url` indicates a path pointing to a navigator view.

## Input ##

| **Parameter** | **Value(s)** |
|:--------------|:-------------|
| cmd           | open\_path   |
| url           | {path}       |
| [[timeout](timeout.md)] | {n}          |

## Output ##

It is impossible to determine the current state of the `url` setting, unless you use a variable to remember the last value.

One way to do this is to assume that the last entered value is the current value.

This method is inefficient because the actual value can be changed outside of your control.

## Values ##

|                        | **Path**                  |
|:-----------------------|:--------------------------|
| **Sources**              | root://sources            |
| **TV**                   | root://tv                 |
| **Audio**                | root://music\_and\_radio  |
| **Video**                | root://video              |
| **Social Networking**    | root://social\_networks   |
| **News and Weather**     | root://news\_and\_weather |
| **Games**                | root://games              |
| **Applications**         | root://applications       |
| **Settings**             | root://setup              |
| **Application Settings** | setup://applications      |

## Notes ##

  * Sub-menus are accessible if you can figure out the proper name.
  * Valid menu paths are language specific. For example: `root://setup/Miscellaneous` is only valid if the system language is English.
  * When using the classic UI (plain icon view), you must still specify menu sections as if you were using the modern UI (tabs/folders view).