namespace Snake
{
    public sealed class GridDescription
    {
        public int GridSize { get; }
        public int RowCount { get; }
        public int ColumnCount { get; }

        public GridDescription(int gridSize, int rowCount, int columnCount)
        {
            GridSize = gridSize;
            RowCount = rowCount;
            ColumnCount = columnCount;
        }
    }
}
