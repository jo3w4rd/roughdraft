

ArrayList<PVector> vertices = new ArrayList<PVector>();

void setup(){
 size(600, 600, P3D);
}

void draw(){
  background(255,255,255);
  stroke(0,0,0);
  PVector normal = new PVector(.3, .4, .3);
 ArrayList<PVector> points = arc(360, new PVector(width/2, height/2, 10), 
                                      new PVector(0, 1, 0), 
                                      normal,
                                      40, 6);
                                      
 IntList triangles = triangulateCylinder(points, 60, normal);                                 
                     
 println(triangles);                 
 beginShape();
 for(PVector point : points){
  vertex(point.x, point.y, point.z); 
 }
 endShape();
}

IntList triangulateCylinder(ArrayList<PVector> points, float cylinderHeight, PVector circleNormal){
   IntList triangles = new IntList();
   int i = 1;
   while( i < points.size()){
     triangles.append(0);
     triangles.append(i);
     triangles.append(i + 1);
     i++;
   }
   int center2 = i;
   i++;
   while( i < points.size() * 2){
     triangles.append(center2);
     triangles.append(i);
     triangles.append(i + 1);
     i++;
   }
   for(int j = 1; j < points.size(); j++){
      triangles.append(j);
      triangles.append(j + center2 + 1);
      triangles.append(j + center2 + 2); 
      triangles.append(j + center2 + 2); 
      triangles.append(j);
      triangles.append(j + 1);
   }
   circleNormal.mult(cylinderHeight);
   int size = points.size();
   for(int p = 0; p < size; p++){
      PVector opposite = new PVector();
      opposite.set(points.get(p));
      opposite.add(circleNormal);
      points.add(opposite); 
   }
   
   return triangles;
}

/* Adapted from: Zarrax (http://math.stackexchange.com/users/3035/zarrax), Parametric Equation of a Circle in 3D Space?, 
* URL (version: 2014-09-09): http://math.stackexchange.com/q/73242 */
ArrayList<PVector> arc(    float arc,
                           PVector center, 
                           PVector forward,
                           PVector normal,
                           float radius, 
                           int facets) {
      ArrayList<PVector> points = new ArrayList<PVector>(facets + 1);
      points.add(center);
      forward.normalize();
      PVector right = normal.cross(forward);
      right.normalize();
      float deltaAngle = arc/facets;
      for(float angle = 0; abs(angle) <= abs(arc); angle += deltaAngle){
        PVector nextPoint = new PVector(0, 0, 0);
        float cosAngle = cos(radians(angle));
        float sinAngle = sin(radians(angle));
        nextPoint.x = center.x + radius * (cosAngle * forward.x + sinAngle * right.x);
        nextPoint.y = center.y + radius * (cosAngle * forward.y + sinAngle * right.y);
        nextPoint.z = center.z + radius * (cosAngle * forward.z + sinAngle * right.z);
        points.add(nextPoint);
      }
      return points;
    }

