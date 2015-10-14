import numpy as np
import cv2
from matplotlib import pyplot as plt

cap = cv2.VideoCapture() # open the default camera
cap.open(0)

# Initiate STAR detector
orb = cv2.ORB(1100)

while True:
    ret, img = cap.read()
    
    gray = cv2.cvtColor(img, cv2.COLOR_BGR2GRAY)
    # find the keypoints with ORB
    kp = orb.detect(img,None)

    # compute the descriptors with ORB
    kp, des = orb.compute(gray, kp)


    # draw only keypoints location,not size and orientation
    img2 = cv2.drawKeypoints(gray,kp,color=(0,255,0), flags=0)
    cv2.imshow("WINDOW", img2)
    cv2.waitKey(16)

# When everything done, release the capture
cap.release()
cv2.destroyAllWindows()