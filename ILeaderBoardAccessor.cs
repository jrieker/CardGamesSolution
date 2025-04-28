using CardGamesSolution.Server.Database;

namespace CardGamesSolution.Server.Leaderboard
{
    public interface ILeaderBoardAccessor
    {
        void GetConnection();
        void UpdateLeaderBoard();
        void SaveLeaderBoardData();
        void UpdateLeaderBoardData();
        void SortLeaderBoard();
        int GetNextLeaderBoardId();
    }
}
