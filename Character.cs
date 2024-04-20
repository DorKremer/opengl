using System;
using System.Collections;
using System.Text.RegularExpressions;
using System.IO;
using MathEX;
using OpenGL;

namespace Milkshape
{
    public class Character : MilkshapeModel
    {
        private AnimationCollection animations;
        /// Gets the AnimationCollection which was generate by the animation configuration file
        public AnimationCollection Animations { get { return animations; } }

        /// Creates a new instance of Character.
        public Character()
        { }

        /// Creates a new instance of Character.
        public Character(string ms3dfile)
        {
            // Loads the main ms3d model
            LoadModelData(ms3dfile);

            // Loads the accompanying ms3dfile .txt adnimation config file.
            LoadConfigFile(ms3dfile.ToLower().Replace(".ms3d", ".txt"));

            // Reset the animation
            Restart();
        }

        /// Loads the animation file and populates the Dialogue class, YOffset and ScaleFactor
        private void LoadConfigFile(string filename)
        {
            // Make sure the file exists
            System.IO.FileInfo fInfo = new System.IO.FileInfo(filename);
            if (!fInfo.Exists)
                return;

            fInfo = null;

            // Open the config file
            System.IO.StreamReader pobjReader = new System.IO.StreamReader(filename, System.Text.Encoding.ASCII);

            // Now read the complete contents.
            string contents = pobjReader.ReadToEnd();
            pobjReader.Close();
            pobjReader = null;

            // Split it up into lines and load the animation settings.
            string[] lines = contents.Split('\n');

            animations = new AnimationCollection();

            for (int i = 0; i < lines.Length; i++)
            {
                if (lines[i].StartsWith("#"))
                    continue;

                Match m = Regex.Match(lines[i], @"^(?<name>\w+)\s+(?<start>\d+)\s+(?<end>\d+)\s+(?<delay>\d+)");
                if (m.Success)
                {
                    Milkshape.Animation anim = new Milkshape.Animation();
                    anim.Name = m.Groups["name"].Value;
                    anim.StartKeyframe = int.Parse(m.Groups["start"].Value);
                    anim.EndKeyFrame = int.Parse(m.Groups["end"].Value);
                    anim.Delay = int.Parse(m.Groups["delay"].Value);
                    anim.StartTime = anim.StartKeyframe * 41;
                    anim.EndTime = anim.EndKeyFrame * 41;
                    animations.Add(anim);
                }
            }

            // Set animations for the model
            base.model_data.Animations = (Milkshape.Animation[])animations.ToArray();
        }

        public override void DrawModel()
        {
            GL.glPushAttrib(GL.GL_ALL_ATTRIB_BITS);

            if (true)
                AdvanceAnimation();
            
            // Draw by group
            for (int i = 0; i < model_data.numMeshes; i++)
            {
                int materialIndex = model_data.Meshes[i].materialIndex;

                BindMaterial(materialIndex);

                GL.glBegin(GL.GL_TRIANGLES);
                for (int j = 0; j < model_data.Meshes[i].numTriangles; j++)
                {
                    int triangleIndex = model_data.Meshes[i].TriangleIndices[j];
                    Triangle pTri = model_data.Triangles[triangleIndex];

                    for (int k = 0; k < 3; k++)
                    {
                        int index = pTri.vertexIndices[k];

                        // rotate according to transformation matrix
                     //   Matrix final = new Matrix(model_data.Joints[model_data.Vertices[index].boneID].mat_final);
                        Matrix final = new Matrix();
                        GL.glTexCoord2f(pTri.s[k], pTri.t[k]);

                        int scalar = (k * 3);
                        float[] norm = new float[3] { pTri.vertexNormals[0 + scalar], pTri.vertexNormals[1 + scalar], pTri.vertexNormals[2 + scalar] };

                        Vector newNormal = new Vector(norm);
                        newNormal.Transform3(final);
                        newNormal.Normalize();
                        GL.glNormal3fv(newNormal.GetVector);

                        Vector newVertex = new Vector(model_data.Vertices[index].location);
                        newVertex.Transform(final);
                        GL.glVertex3fv(newVertex.GetVector);
                    }
                }
                GL.glEnd();
            }

            GL.glPopAttrib();
        }

        #region Animation
        /// Plays an animation which was defined in your .txt config file.
        public void PlayAnimation(string name)
        {
            if (model_data.Animations == null)
                return;

            for (int i = 0; i < model_data.Animations.Length; i++)
            {
                if (model_data.Animations[i].Name.ToLower() == name.ToLower())
                {
                    base.currentAnimation = model_data.Animations[i];
                    Restart();
                    Animate = true;
                    return;
                }
            }
        }

        /// Stops animating the character
        public void Stop()
        {
            Restart();
            Animate = false;
        }

        /// Resets model pose to frame 0
        private void Restart()
        {
            int startKeyframe = 0;

            for (int i = 0; i < model_data.numJoints; i++)
            {
                model_data.Joints[i].currentRotationKeyframe = startKeyframe;
                model_data.Joints[i].currentTranslationKeyframe = startKeyframe;
                model_data.Joints[i].mat_final.Set(model_data.Joints[i].mat_absolute);
            }
            lastTime = DateTime.Now;
        }

        /// Pushed pose for next frame based on time
        private void AdvanceAnimation()
        {
            if (currentAnimation != null)
            {
                TimeSpan ts = (DateTime.Now - lastTime);
                double time = ts.TotalMilliseconds / currentAnimation.Delay;

                double startTime = 0;
                double endTime = model_data.totalTime;

                startTime = currentAnimation.StartTime;
                endTime = currentAnimation.EndTime;
                time += startTime;

                if (time > endTime)
                {
                    Restart();
                    time = startTime;
                }

                if (time < endTime)
                {
                    #region Translate & Rotate
                    for (int i = 0; i < model_data.numJoints; i++)
                    {
                        float[] transVec = new float[3];
                        Matrix mat_transform = new Matrix();
                        int frame = 0;
                        Joint pJoint = model_data.Joints[i];

                        if (pJoint.numRotationKeyframes == 0
                            && pJoint.numTranslationKeyframes == 0)
                        {
                            pJoint.mat_final.Set(pJoint.mat_absolute);
                            continue;
                        }

                        frame = pJoint.currentTranslationKeyframe;

                        while (frame < pJoint.numTranslationKeyframes
                            && pJoint.TranslationKeyframes[frame].time < time)
                        {
                            frame++;
                        }

                        pJoint.currentTranslationKeyframe = frame;

                        if (frame == 0)
                            transVec = pJoint.TranslationKeyframes[0].parameter;
                        else if (frame == pJoint.numTranslationKeyframes)
                            transVec = pJoint.TranslationKeyframes[frame - 1].parameter;
                        else
                        {
                            Keyframe curFrame = pJoint.TranslationKeyframes[frame];
                            Keyframe prevFrame = pJoint.TranslationKeyframes[frame - 1];

                            float timeDelta = curFrame.time - prevFrame.time;
                            float interpValue = (float)(((float)time - (float)prevFrame.time) / (float)timeDelta);

                            transVec[0] = prevFrame.parameter[0] + (curFrame.parameter[0] - prevFrame.parameter[0]) * interpValue;
                            transVec[1] = prevFrame.parameter[1] + (curFrame.parameter[1] - prevFrame.parameter[1]) * interpValue;
                            transVec[2] = prevFrame.parameter[2] + (curFrame.parameter[2] - prevFrame.parameter[2]) * interpValue;
                        }

                        frame = pJoint.currentRotationKeyframe;

                        while (frame < pJoint.numRotationKeyframes
                            && pJoint.RotationKeyframes[frame].time < time)
                        {
                            frame++;
                        }

                        pJoint.currentRotationKeyframe = frame;

                        if (frame == 0)
                            mat_transform.SetRotationRadians(pJoint.RotationKeyframes[0].parameter);
                        else if (frame == pJoint.numRotationKeyframes)
                            mat_transform.SetRotationRadians(pJoint.RotationKeyframes[frame - 1].parameter);
                        else
                        {
                            Keyframe curFrame = pJoint.RotationKeyframes[frame];
                            Keyframe prevFrame = pJoint.RotationKeyframes[frame - 1];

                            float timeDelta = curFrame.time - prevFrame.time;
                            float interpValue = (float)((time - prevFrame.time) / timeDelta);

                            float[] rotVec = new float[3];

                            rotVec[0] = prevFrame.parameter[0] + (curFrame.parameter[0] - prevFrame.parameter[0]) * interpValue;
                            rotVec[1] = prevFrame.parameter[1] + (curFrame.parameter[1] - prevFrame.parameter[1]) * interpValue;
                            rotVec[2] = prevFrame.parameter[2] + (curFrame.parameter[2] - prevFrame.parameter[2]) * interpValue;

                            mat_transform.SetRotationRadians(rotVec);
                        }

                        mat_transform.SetTranslation(transVec);

                        Matrix relativeFinal = new Matrix(pJoint.mat_relative);
                        relativeFinal.PostMultiply(mat_transform);

                        if (pJoint.parent == -1)
                            pJoint.mat_final.Set(relativeFinal);
                        else
                        {
                            pJoint.mat_final.Set(model_data.Joints[pJoint.parent].mat_final);
                            pJoint.mat_final.PostMultiply(relativeFinal);
                        }
                    }
                    #endregion
                }
            }
        }
        #endregion
    }

    public class AnimationCollection : IList
    {
        private ArrayList items = new ArrayList();

        public Animation[] ToArray()
        {
            return (Animation[])items.ToArray(typeof(Animation));
        }

        #region IList Members

        public bool IsReadOnly
        {
            get
            {
                return items.IsReadOnly;
            }
        }

        object IList.this[int index]
        {
            get
            {
                return items[index];
            }
            set
            {
                items[index] = value;
            }
        }

        public Animation this[string name]
        {
            get
            {
                for (int i = 0; i < items.Count; i++)
                {
                    if (((Milkshape.Animation)items[i]).Name.ToLower() == name.ToLower())
                        return (Milkshape.Animation)items[i];
                }
                return null;
            }
            set
            {
                for (int i = 0; i < items.Count; i++)
                {
                    if (((Milkshape.Animation)items[i]).Name.ToLower() == name.ToLower())
                    {
                        items[i] = (Milkshape.Animation)value;
                        break;
                    }
                }
            }
        }

        public virtual Animation this[int index]
        {
            get
            {
                return (Milkshape.Animation)items[index];
            }
            set
            {
                items[index] = value;
            }
        }

        public void RemoveAt(int index)
        {
            items.RemoveAt(index);
        }

        public void Insert(int index, object value)
        {
            items.Insert(index, value);
        }

        public void Remove(object value)
        {
            items.Remove(value);
        }

        public bool Contains(object value)
        {
            return items.Contains(value);
        }

        public bool Contains(string name)
        {
            for (int i = 0; i < items.Count; i++)
            {
                if (((Milkshape.Animation)items[i]).Name.ToLower() == name.ToLower())
                    return true;
            }
            return false;
        }

        public void Clear()
        {
            items.Clear();
        }

        public int IndexOf(object value)
        {
            return items.IndexOf(value);
        }

        public int Add(object value)
        {
            return items.Add(value);
        }

        public int Add(Animation value)
        {
            return items.Add(value);
        }

        public bool IsFixedSize
        {
            get
            {
                return items.IsFixedSize;
            }
        }

        #endregion

        #region ICollection Members

        public bool IsSynchronized
        {
            get
            {
                return items.IsSynchronized;
            }
        }

        public int Count
        {
            get
            {
                return items.Count;
            }
        }

        public void CopyTo(Array array, int index)
        {
            items.CopyTo(array, index);
        }

        public object SyncRoot
        {
            get
            {
                return items.SyncRoot;
            }
        }

        #endregion

        #region IEnumerable Members

        public IEnumerator GetEnumerator()
        {
            return items.GetEnumerator();
        }

        #endregion
    }
}