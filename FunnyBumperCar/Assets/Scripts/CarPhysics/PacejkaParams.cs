

/**
 * 
    Coefficient	Name	Typical range	Typical values for longitudinal forces
    Dry tarmac	Wet tarmac	Snow	Ice
    B	Stiffness	4 .. 12	10	12	5	4
    C*	Shape	1 .. 2	1.9	2.3	2	2
    D	Peak	0.1 .. 1.9	1	0.82	0.3	0.1
    E	Curvature	-10 .. 1	0.97	1	1	1
 */
public struct PacejkaParams
{
    public float B_Stiffness;
    public float C_Shape;
    public float D_Peak;
    public float E_Curvature;
}