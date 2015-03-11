
int dWidth = 600;
int dHeight = 800;

void setup()
{
  size(dWidth, dHeight);
}

void draw()
{
   //drawShape( 0, 30, color(244,34,34)); 
   //drawShape( 30, 30, color(34,34,128)); 
   //drawShape( 60, 30, color(34,128,34)); 
   drawShape( 90, 30, color(244,34,34)); 
   //drawShape( 120, 30, color(34,34,128)); 
   //drawShape( 150, 30, color(34,128,34)); 

}

void drawShape(float startAngle, float sweep, int fillColor)
{
   fill(fillColor);
   beginShape();
   vertex(dWidth/2, dHeight/2);
   Point start = intercept((float)Math.toRadians(startAngle));
   vertex(start.x, start.y);
   Point stop = intercept((float)Math.toRadians(startAngle + sweep));
   vertex(stop.x, stop.y);
   vertex(dWidth/2, dHeight/2);
   endShape();
}

Point intercept(float radians)
{
   Point result = new Point(0,0);
   
   if(radians >= 0 && radians < Math.PI/2)
   {
        result.y = 0;
        result.x = .5 * dHeight * (1 - (float)(Math.tan(Math.PI/4 - radians))); 
    } else if ( radians > Math.PI/2 && radians <= Math.PI)
    {
        result.x = dWidth;
        result.y = .5 * dWidth * (1 - (float)(Math.tan(Math.PI/4 - radians))); 
    }  else if ( radians >= Math.PI && radians < 1.5 * Math.PI)
    {
        result.y = dHeight;
        result.x = .5 * dHeight * (1 - (float)(Math.tan(Math.PI/4 - radians))); 
    }  else if ( radians >= 1.5 * Math.PI && radians < 2 * Math.PI)
    {
        result.x = 0;
        result.y = .5 * dWidth * (1 - (float)(Math.tan(Math.PI/4 - radians))); 
    } 
    
    return result;
  
}
