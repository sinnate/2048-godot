extends Panel
var value = 0 setget setCell,GetValue
var colors = {
# Empty
 0:Color(1.0,0.9,0.85,0.4), 
# ------ 
 2:Color(1.0,0.9,0.85),
 4:Color(1.0,0.85,0.8),
 8:Color(0.95,0.70,0.45),
 16:Color(0.95,0.6,0.4),
 32:Color(0.95,0.5,0.38),
 64:Color(0.95,0.35,0.25),
 128:Color(0.9,0.85,0.45),
 256:Color(0.9,0.8,0.4),
 512:Color(0.9,0.75,0.3),
 1024:Color(0.9,0.7,0.25),
 2048:Color(0.9,0.7,0.2)
}

func setCell(V):
	value = V
	self_modulate = colors[value]
	if value == 0 :
		$LB.text = str("")
	else:
		$LB.text = str(value)
		$Ap.play("cell")
func GetValue() -> int:
	return value