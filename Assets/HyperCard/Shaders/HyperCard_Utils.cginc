half3 AdjustContrast(half3 color, half contrast) 
{
	color = saturate(lerp(half3(0.5, 0.5, 0.5), color, contrast));
	return color;
}