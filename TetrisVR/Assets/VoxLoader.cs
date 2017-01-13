
#define DEBUG

using UnityEngine;
using System.Collections;
using System.IO;
using UnityEditor;

public class VoxLoader : MonoBehaviour
{
    public class MaterialIndex : MonoBehaviour
    {
        public int Index { get; set; }
    }

    private class ParseException : System.Exception
    {
        public ParseException(string message) : base(message) { }
        public ParseException(string message, System.Exception innerException) : base(message, innerException) { }
    }

    public string ModelPath;
    public string OutputPath = "Assets/Prefabs/Fractured/";
    public string MaterialsDst = "Assets/Materials/vox/";
    public string shaderName = "Standard";

    const string VOX_SIGNATURE = "VOX ";
    const int VOX_MIN_SUPPORTED_VERSION = 150;
    const int VOX_MAX_SUPPORTED_VERSION = 150;

    const int CHUNK_HEADER_SIZE = 12;

    const string CHUNK_MAIN_ID = "MAIN";
    const string CHUNK_PACK_ID = "PACK";
    const string CHUNK_SIZE_ID = "SIZE";
    const int CHUNK_SIZE_SIZE = 24;
    const string CHUNK_XYZI_ID = "XYZI";
    const string CHUNK_RGBA_ID = "RGBA";
    const int CHUNK_RGBA_SIZE = 256 * 4 + 12;

    readonly uint[] default_palette = {
        0x00000000, 0xffffffff, 0xffccffff, 0xff99ffff, 0xff66ffff, 0xff33ffff, 0xff00ffff, 0xffffccff, 0xffccccff, 0xff99ccff, 0xff66ccff, 0xff33ccff, 0xff00ccff, 0xffff99ff, 0xffcc99ff, 0xff9999ff,
        0xff6699ff, 0xff3399ff, 0xff0099ff, 0xffff66ff, 0xffcc66ff, 0xff9966ff, 0xff6666ff, 0xff3366ff, 0xff0066ff, 0xffff33ff, 0xffcc33ff, 0xff9933ff, 0xff6633ff, 0xff3333ff, 0xff0033ff, 0xffff00ff,
        0xffcc00ff, 0xff9900ff, 0xff6600ff, 0xff3300ff, 0xff0000ff, 0xffffffcc, 0xffccffcc, 0xff99ffcc, 0xff66ffcc, 0xff33ffcc, 0xff00ffcc, 0xffffcccc, 0xffcccccc, 0xff99cccc, 0xff66cccc, 0xff33cccc,
        0xff00cccc, 0xffff99cc, 0xffcc99cc, 0xff9999cc, 0xff6699cc, 0xff3399cc, 0xff0099cc, 0xffff66cc, 0xffcc66cc, 0xff9966cc, 0xff6666cc, 0xff3366cc, 0xff0066cc, 0xffff33cc, 0xffcc33cc, 0xff9933cc,
        0xff6633cc, 0xff3333cc, 0xff0033cc, 0xffff00cc, 0xffcc00cc, 0xff9900cc, 0xff6600cc, 0xff3300cc, 0xff0000cc, 0xffffff99, 0xffccff99, 0xff99ff99, 0xff66ff99, 0xff33ff99, 0xff00ff99, 0xffffcc99,
        0xffcccc99, 0xff99cc99, 0xff66cc99, 0xff33cc99, 0xff00cc99, 0xffff9999, 0xffcc9999, 0xff999999, 0xff669999, 0xff339999, 0xff009999, 0xffff6699, 0xffcc6699, 0xff996699, 0xff666699, 0xff336699,
        0xff006699, 0xffff3399, 0xffcc3399, 0xff993399, 0xff663399, 0xff333399, 0xff003399, 0xffff0099, 0xffcc0099, 0xff990099, 0xff660099, 0xff330099, 0xff000099, 0xffffff66, 0xffccff66, 0xff99ff66,
        0xff66ff66, 0xff33ff66, 0xff00ff66, 0xffffcc66, 0xffcccc66, 0xff99cc66, 0xff66cc66, 0xff33cc66, 0xff00cc66, 0xffff9966, 0xffcc9966, 0xff999966, 0xff669966, 0xff339966, 0xff009966, 0xffff6666,
        0xffcc6666, 0xff996666, 0xff666666, 0xff336666, 0xff006666, 0xffff3366, 0xffcc3366, 0xff993366, 0xff663366, 0xff333366, 0xff003366, 0xffff0066, 0xffcc0066, 0xff990066, 0xff660066, 0xff330066,
        0xff000066, 0xffffff33, 0xffccff33, 0xff99ff33, 0xff66ff33, 0xff33ff33, 0xff00ff33, 0xffffcc33, 0xffcccc33, 0xff99cc33, 0xff66cc33, 0xff33cc33, 0xff00cc33, 0xffff9933, 0xffcc9933, 0xff999933,
        0xff669933, 0xff339933, 0xff009933, 0xffff6633, 0xffcc6633, 0xff996633, 0xff666633, 0xff336633, 0xff006633, 0xffff3333, 0xffcc3333, 0xff993333, 0xff663333, 0xff333333, 0xff003333, 0xffff0033,
        0xffcc0033, 0xff990033, 0xff660033, 0xff330033, 0xff000033, 0xffffff00, 0xffccff00, 0xff99ff00, 0xff66ff00, 0xff33ff00, 0xff00ff00, 0xffffcc00, 0xffcccc00, 0xff99cc00, 0xff66cc00, 0xff33cc00,
        0xff00cc00, 0xffff9900, 0xffcc9900, 0xff999900, 0xff669900, 0xff339900, 0xff009900, 0xffff6600, 0xffcc6600, 0xff996600, 0xff666600, 0xff336600, 0xff006600, 0xffff3300, 0xffcc3300, 0xff993300,
        0xff663300, 0xff333300, 0xff003300, 0xffff0000, 0xffcc0000, 0xff990000, 0xff660000, 0xff330000, 0xff0000ee, 0xff0000dd, 0xff0000bb, 0xff0000aa, 0xff000088, 0xff000077, 0xff000055, 0xff000044,
        0xff000022, 0xff000011, 0xff00ee00, 0xff00dd00, 0xff00bb00, 0xff00aa00, 0xff008800, 0xff007700, 0xff005500, 0xff004400, 0xff002200, 0xff001100, 0xffee0000, 0xffdd0000, 0xffbb0000, 0xffaa0000,
        0xff880000, 0xff770000, 0xff550000, 0xff440000, 0xff220000, 0xff110000, 0xffeeeeee, 0xffdddddd, 0xffbbbbbb, 0xffaaaaaa, 0xff888888, 0xff777777, 0xff555555, 0xff444444, 0xff222222, 0xff111111
    };

    private void setVoxelColor(GameObject voxel, Material[] materials)
    {
        MaterialIndex mi = voxel.GetComponent<MaterialIndex>();
        int index = mi.Index;
        DestroyImmediate(mi);
        Material mat = materials[index];
        voxel.GetComponent<Renderer>().sharedMaterial = mat;
        string path = AssetDatabase.GetAssetPath(mat.GetInstanceID());
        if (path == null)
        {
            AssetDatabase.CreateAsset(mat, OutputPath + "_" + index + ".mat");
        }
        else if (path.Length == 0)
        {
            AssetDatabase.CreateAsset(mat, OutputPath + "_" + index + ".mat");
        }
    }

    private string readChunkHeader(BinaryReader binReader)
    {
        char[] chunkId = binReader.ReadChars(4);
        binReader.BaseStream.Seek(CHUNK_HEADER_SIZE - 4, SeekOrigin.Current);

        return new string(chunkId);
    }

    private void checkFileType(BinaryReader binReader)
    {
        string signature = new string(binReader.ReadChars(4));

        if (!signature.Equals(VOX_SIGNATURE))
        {
            throw new ParseException("Signature incorrecte : " + signature);
        }

        int version = binReader.ReadInt32();

        if (version < VOX_MIN_SUPPORTED_VERSION || version > VOX_MAX_SUPPORTED_VERSION)
        {
            throw new ParseException(
                string.Format("Version {0} du format non supportée (min={1}, max={2})",
                    version, VOX_MIN_SUPPORTED_VERSION, VOX_MAX_SUPPORTED_VERSION));
        }

        string chunkId = readChunkHeader(binReader);

        if (!chunkId.Equals(CHUNK_MAIN_ID))
        {
            throw new ParseException(string.Format("Chunk MAIN attendu, trouvé {0} à la place.", chunkId));
        }
    }

    private void readModels(BinaryReader binReader)
    {
        int modelCount = 1;

        string chunkId = readChunkHeader(binReader);

        if (chunkId.Equals(CHUNK_PACK_ID))
        {
            modelCount = binReader.ReadInt32();
            chunkId = readChunkHeader(binReader);
        }

        print(modelCount);

        for (int i = 0; i < modelCount; i++)
        {
            if (!chunkId.Equals(CHUNK_SIZE_ID))
            {
                throw new ParseException(string.Format("Chunk SIZE attendu, trouvé {0} à la place.", chunkId));
            }

            binReader.BaseStream.Seek(CHUNK_SIZE_SIZE - CHUNK_HEADER_SIZE, SeekOrigin.Current);

            chunkId = readChunkHeader(binReader);

            if (!chunkId.Equals(CHUNK_XYZI_ID))
            {
                throw new ParseException(string.Format("Chunk XYZI attendu, trouvé {0} à la place.", chunkId));
            }

            GameObject currentModel = new GameObject();
            currentModel.transform.parent = this.transform;

            int voxelCount = binReader.ReadInt32();
            print(voxelCount);

            for (int j = 0; j<voxelCount; j++)
            {
                float x, y, z;

                GameObject voxel = GameObject.CreatePrimitive(PrimitiveType.Cube);
                voxel.transform.parent = currentModel.transform;

                x = (float)binReader.ReadByte();
                y = (float)binReader.ReadByte();
                z = (float)binReader.ReadByte();

                voxel.transform.position = new Vector3(x, z, y);
                MaterialIndex material = voxel.AddComponent<MaterialIndex>();
                material.Index = binReader.ReadByte();
                voxel.AddComponent<BoxCollider>();
                voxel.AddComponent<Rigidbody>();
            }
        }
    }

    private uint[] readPalette(BinaryReader binReader)
    {
        string chunkId = readChunkHeader(binReader);

        if (!chunkId.Equals(CHUNK_RGBA_ID))
        {
            print("Using default palette");
            return default_palette;
        }

        uint[] palette = new uint[256];
        
        for (int i=0; i<254; i++)
        {
            palette[i + 1] = binReader.ReadUInt32();
        }

        return palette;
    }

    Material[] materialsFromPalette(uint[] palette, Shader shader)
    {
        Material[] materials = new Material[256];
        for (int i=0; i<256; i++)
        {
            materials[i] = new Material(shader);

            uint uintColor = palette[i];
            byte r = (byte)(uintColor >> 0);
            byte g = (byte)(uintColor >> 8);
            byte b = (byte)(uintColor >> 16);

            Debug.Log(string.Format("{0:X}", uintColor));
            print(r + "," + g + "," + b);

            materials[i].SetColor("_Color", new Color32(r, g, b, 255));
        }

        return materials;
    }

    [ContextMenu("Load .vox model")]
    bool loadVoxModel()
    {
        if (!File.Exists(ModelPath))
        {
            print("Le fichier " + ModelPath + " n'éxiste pas.");

            return false;
        }

        using (FileStream br = new FileStream(ModelPath, FileMode.Open, FileAccess.Read, FileShare.Read))
        {
            byte[] buffer = new byte[br.Length];
            br.Read(buffer, 0, buffer.Length);

            using (MemoryStream memoryStream = new MemoryStream(buffer))
            {
                using (BinaryReader binReader = new BinaryReader(memoryStream))
                {
                    try
                    {
                        int i = 1;
                        checkFileType(binReader);
                        readModels(binReader);
                        uint[] palette = readPalette(binReader);
                        Material[] materials = materialsFromPalette(palette, Shader.Find(shaderName));

                        foreach (Transform model in transform)
                        {
                            foreach (Transform voxel in model)
                            {
                                setVoxelColor(voxel.gameObject, materials);
                            }

                            AssetDatabase.SaveAssets();
                            PrefabUtility.CreatePrefab(OutputPath + "_" + i++ + ".prefab", model.gameObject);
                            DestroyImmediate(model.gameObject);
                        }

                        AssetDatabase.SaveAssets();
                    }
                    catch (ParseException e)
                    {
                        print("Le fichier est corrompu (" + e.Message + ")");
                    }
                    catch (EndOfStreamException e)
                    {
                        print("Fin de fichier inattendue : le fichier est corrompu (" + e.Message + ")");

                        // clean up everything that has been loaded already

                        // load another file if any

                        return false;
                    }

                    binReader.Close();
                }
                memoryStream.Close();
            }
            br.Close();
        }

        return true;
    }
}