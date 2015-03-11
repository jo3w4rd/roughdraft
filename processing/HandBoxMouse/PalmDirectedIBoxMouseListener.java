package com.leapmotion.example;

import com.leapmotion.leap.*;

import java.awt.AWTException;
import java.awt.MouseInfo;
import java.awt.Point;
import java.awt.PointerInfo;
import java.awt.Rectangle;
import java.awt.Robot;
import java.awt.event.InputEvent;

public class PalmDirectedIBoxMouseListener extends Listener {
	  Robot robot;
	  Point last = new Point();
	  int handID = -1;
	  int screenWidth = 0;
	  int screenHeight = 0;
	  float lastTouch = 2;
	  int boxSize = 400;
	  int boxX = 0;
	  int boxY = 0;
	  int extraBoxHeight = 0;
	  boolean touching = false;
	  float baseZ = 0;
	  
	  public PalmDirectedIBoxMouseListener() {
		  super();
		  try {
			robot = new Robot();
			screenWidth = java.awt.Toolkit.getDefaultToolkit().getScreenSize().width;
			screenHeight = java.awt.Toolkit.getDefaultToolkit().getScreenSize().height;
		} catch (AWTException e) {
			e.printStackTrace();
		}
	  }
	  public void onInit(Controller controller) {
		    System.out.println("Initialized");
		    controller.setPolicyFlags(Controller.PolicyFlag.POLICY_BACKGROUND_FRAMES);
		  }

		  public void onConnect(Controller controller) {
		    System.out.println("Connected");

		  }

		  public void onDisconnect(Controller controller) {
		    System.out.println("Disconnected");
		  }

		  public void onFrame(Controller controller) {
			  try {
					PointerInfo info = MouseInfo.getPointerInfo();
				  	Point mouse = info.getLocation();
				  	Frame frame = controller.frame(); 
				    InteractionBox iBox = frame.interactionBox();
				    
				    Pointable pointer = frame.pointables().frontmost();
				    Vector normalizedPointerPosition = iBox.normalizePoint( pointer.tipPosition(), false );
				    //Vector normalizedHandPosition = iBox.normalizePoint( pointer.hand().palmPosition().plus(pointer.hand().direction().times(pointer.length() * 1.5f)) );
				    Vector  normalizedHandPosition = iBox.normalizePoint( pointer.stabilizedTipPosition().minus(pointer.direction().times(pointer.length() * 1.0f)) );

				    if( pointer.isValid() && pointer.hand().isValid() )
				    {
				        int handX = (int) (screenWidth * normalizedHandPosition.getX());
				        int handY = (int) (screenHeight - screenHeight * normalizedHandPosition.getY());
				        if( (handX > boxX + boxSize) ) boxX = handX - boxSize;
				        if( (handX < boxX) ) boxX = handX;
				        if( (handY > boxY + boxSize) ) boxY = handY - boxSize;
				        if( (handY < boxY) ) boxY = handY;
				        
				        int pointerX = (int) (boxX + boxSize * normalizedPointerPosition.getX());
				        int pointerY = (int) (boxY + boxSize - extraBoxHeight/2 - (boxSize + extraBoxHeight) * normalizedPointerPosition.getY());
				        
				        //System.out.println("Tip y velocity: " + pointer.tipVelocity().getY());
				        //System.out.println("Tip z velocity^2: " + pointer.tipVelocity().getZ() * pointer.tipVelocity().getZ());
				        //System.out.println("Hand velocity squared: " + pointer.hand().palmVelocity().magnitudeSquared());
				        //if((pointer.tipVelocity().getY() > - 200) || pointer.tipVelocity().getZ() > - 200 || pointer.hand().palmVelocity().magnitudeSquared() > 30000)
				        /*
				        int fudge = 1;
				        if(pointerX < boxX) boxX -= (boxX - pointerX)/fudge;
				        if(pointerX > boxX + boxSize) boxX += (pointerX - boxX - boxSize)/fudge;
				        if(pointerY < boxY) boxY -= (boxY - pointerY)/fudge;
				        if(pointerY > boxY + boxSize) boxY += (pointerY - boxY - boxSize)/fudge;
				        */
				        if(pointerX >= screenWidth) pointerX = screenWidth - 1;
				        if(pointerX < 0) pointerX = 0;
				        if(pointerY >= screenHeight) pointerY = screenHeight - 1;
				        if(pointerY < 0) pointerY = 0;
				        
				        robot.mouseMove(pointerX, pointerY);
				        
				        final float clickDistance = 5;
				        baseZ = getBaseZ(pointer.id(), 20, controller);
				        if( (pointer.tipPosition().getZ() < baseZ - clickDistance) && !touching)
				        {
				        	touching = true;
				  			robot.mousePress(InputEvent.BUTTON1_MASK);
				  			System.out.println("Press");
				        }
				        if( (pointer.tipPosition().getZ() > baseZ + clickDistance) && touching)
				        {
				        	touching = false;
				  			robot.mouseRelease(InputEvent.BUTTON1_MASK);
				  			System.out.println("Release");
				        }
				        
				        /*
					  		if(pointer.touchDistance() <= 0 && lastTouch > 0)
					  		{
					  			robot.mousePress(InputEvent.BUTTON1_MASK);
					  			System.out.println("Press");
					  		}
					  		*/
				        }
				    		/*
					  		if(pointer.touchDistance() > 0 && lastTouch <= 0)
					  		{
					  			robot.mouseRelease(InputEvent.BUTTON1_MASK);
					  			System.out.println("Release");
					  		}
					  		lastTouch = pointer.touchDistance();
					  		*/
				  
			  } catch(Exception e) {
				  System.out.println(e.getClass().getName() + ", " + e.getMessage());
			  }
		  }
		  
		  public float getBaseZ( int id, int samples, Controller controller){
			  float average = 0;
			  float count = 0;
			  for( int s = 0; s < samples; s++)
			  {
				  Pointable pointer = controller.frame(s).pointable(id);
				  if(pointer.isValid())
				  {
					  average += pointer.tipPosition().getZ();
					  count++;
				  }
			  }
			  if(count > 0)
			  {
				  average = average/count;
			  }
			  return average;
		  }
}
