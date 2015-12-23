#summary set\_playback\_state command reference.
#sidebar ApiReferenceSidebar

# set\_playback\_state #
## Summary ##

`set_playback_state` is used to set various playback settings, such as playback position, speed, and volume. Some of the parameters for this command must be combined in a specific way. I have categorized them, but you can try combining parameters from different categories in one request.

## Parameters: General ##

[Go to main article](SetPlaybackStateGeneral.md)

## Parameters: Playback Position ##

[Go to main article](SetPlaybackStatePlaybackPosition.md)

## Parameters: Video Zoom ##

[Go to main article](SetPlaybackStateVideoZoom.md)

## Parameters: Clipping Region ##

[Go to main article](SetPlaybackStateClippingRegion.md)

## Parameters: Teletext ##

[Go to main article](SetPlaybackStateTeletext.md)

## Notes ##

There are some slight differences between input/output parameters names. For example: the status command returns a parameter "`playback_volume`", but you would use the parameter "`volume`" to change its value.