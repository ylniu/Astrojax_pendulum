import numpy as np
m1 = 0.03188
m2 = 0.03188
g = 9.8
l0 = 0.61
theta1 = np.pi/6 
T = (m1 + m2)*g/np.cos(theta1)
cos_theta2 =  m2*np.cos(theta1)/(m1 + m2)
sin_theta2 = (1 - cos_theta2*cos_theta2)**0.5
l1 = l0*sin_theta2/((m1/m2)*sin_theta2+sin_theta2+np.sin(theta1))
l2 = l0 -l1
w = (T/(m1*l1))**0.5
Damping = 0.05
L0 = l0
G        = g
X0x      = 0.0
X0y      = 0.0
X0z      = 0.0

V0x      = 0.0
V0y      = 0.0
V0z      = 0.0

X1x      = X0x + l1 * np.sin(theta1)
X1y      = X0y - l1 * np.cos(theta1)
X1z      = 0.0

V1x      = 0.0
V1y      = 0.0
V1z      = w*l1*np.sin(theta1)

X2x      = X1x + l2 * sin_theta2
X2y      = X1y - l2 * cos_theta2
X2z      = 0.0

V2x      = 0.0
V2y      = 0.0
V2z      = - w*l2*sin_theta2


with open("test.txt","w") as f:
    f.write("Damping = %f\n"%Damping)
    f.write("L0      = %f\n"%L0 )
    f.write("G       = %f\n"%G  )
    f.write("X0x     = %f\n"%X0x)
    f.write("X0y     = %f\n"%X0y)
    f.write("X0z     = %f\n"%X0z)
    f.write("V0x     = %f\n"%V0x)
    f.write("V0y     = %f\n"%V0y)
    f.write("V0z     = %f\n"%V0z)
    f.write("X1x     = %f\n"%X1x)
    f.write("X1y     = %f\n"%X1y)
    f.write("X1z     = %f\n"%X1z)
    f.write("V1x     = %f\n"%V1x)
    f.write("V1y     = %f\n"%V1y)
    f.write("V1z     = %f\n"%V1z)
    f.write("X2x     = %f\n"%X2x)
    f.write("X2y     = %f\n"%X2y)
    f.write("X2z     = %f\n"%X2z)
    f.write("V2x     = %f\n"%V2x)
    f.write("V2y     = %f\n"%V2y)
    f.write("V2z     = %f\n"%V2z)
