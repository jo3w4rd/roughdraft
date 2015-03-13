
public class HandPoseLibrary{
   public ArrayList<HandPose> poses = new ArrayList<HandPose>();
   public int size = 0;
  public boolean add(HandPose pose){
     poses.add(pose);
     size += 1;
     println("Poses in library: " + poses.size());
     return true;
  } 
}
