public class HandPoseRecognizer{
  
   public PoseCheckResult recognize(HandPose poseToCheck, HandPoseLibrary library){
     PoseCheckResult result = new PoseCheckResult();
     if(library.size == 0) return result;
     float bestScore = Float.POSITIVE_INFINITY;
     HandPose bestPose = null;
     for(HandPose template : library.poses){
       float score = GreedyCloudMatch(template, poseToCheck);
       if(score < bestScore){
           bestScore = score;
           bestPose = template;
       }
     }
     result.bestPose = bestPose;
     result.score = bestScore;
     result.failed = false;
     return result;
   }
  
  private float GreedyCloudMatch(HandPose template, HandPose toCheck)
  {
    return CloudDistance(toCheck.jointPositions, template.jointPositions);
  }

  private float CloudDistance(ArrayList<Vector> joints1, ArrayList<Vector> joints2)
  {
    float error = 0;
    for(int p = 0; p < joints1.size(); p++){
        error += joints1.get(p).distanceTo(joints2.get(p));
    }
    return error;
  }   
}
