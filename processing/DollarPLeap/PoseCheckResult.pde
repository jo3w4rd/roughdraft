public class PoseCheckResult{
  public HandPose bestPose;
  public double score = 0;
  public double milliseconds = 0;
  public boolean failed = true;
  
  public String toString(){
     String str = bestPose.name + " score: " + score + (failed ? " failed" : " success");
     return str;  
  }
}
