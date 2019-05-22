namespace GSImporter
{
    public static class GSImporterCellExtension
    {
        public static bool IsNullOrEmpty(this Cell cell)
        {
            return cell == null || cell.v == null;
        }

        public static int GetAsInt(this Cell cell)
        {
            int value;
            return int.TryParse(cell.v, out value) ? value : 0;
        }
        
        public static float GetAsFloat(this Cell cell)
        {
            float value;
            return float.TryParse(cell.v, out value) ? value : 0;
        }
    }
}