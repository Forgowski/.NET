using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BD.Data.Migrations
{
    /// <inheritdoc />
    public partial class dodaniequestion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Question",
                columns: table => new
                {
                    QuestionId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    QuizId = table.Column<int>(type: "int", nullable: false),
                    QuestionText = table.Column<string>(type: "varchar(100)", nullable: false),
                    AnswerA = table.Column<string>(type: "varchar(100)", nullable: false),
                    AnswerB = table.Column<string>(type: "varchar(100)", nullable: false),
                    AnswerC = table.Column<string>(type: "varchar(100)", nullable: false),
                    AnswerD = table.Column<string>(type: "varchar(100)", nullable: false),
                    AnswerCorrect = table.Column<string>(type: "varchar(100)", nullable: false),
                    Point = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Question", x => x.QuestionId);
                    table.ForeignKey(
                        name: "FK_Question_Quiz_QuizId",
                        column: x => x.QuizId,
                        principalTable: "Quiz",
                        principalColumn: "QuizId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Question_QuizId",
                table: "Question",
                column: "QuizId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Question");
        }
    }
}
