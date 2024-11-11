using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Districts",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Titile = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Districts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Logbooks",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    IpAddress = table.Column<string>(type: "text", nullable: false),
                    AccessAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Logbooks", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Number = table.Column<string>(type: "text", nullable: false),
                    Weight = table.Column<double>(type: "double precision", nullable: false),
                    DistrictId = table.Column<Guid>(type: "uuid", nullable: false),
                    DeliveryTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Orders_Districts_DistrictId",
                        column: x => x.DistrictId,
                        principalTable: "Districts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Orders_DistrictId",
                table: "Orders",
                column: "DistrictId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_Number",
                table: "Orders",
                column: "Number",
                unique: true);

            #region Insert Test Data

            Guid[] districtIds =
            [
                Guid.NewGuid(),
                Guid.NewGuid(),
                Guid.NewGuid(),
            ];

            migrationBuilder.InsertData(table: "Districts",
                                            columns: ["Id", "Titile"],
                                            values: new object[,]
                                            {
                                              { districtIds[0], "Центральный район" },
                                              { districtIds[1], "Ленинский район" },
                                              { districtIds[2], "Октябрьский район" }
                                            });

            migrationBuilder.InsertData(table: "Logbooks",
                                          columns: ["Id", "IpAddress", "AccessAt"],
                                          values: new object[,]
                                          {
                                          {
                                                  Guid.NewGuid(),
                                                  "202.44.131.228",
                                                  new DateTime(2023, 12, 15, 10, 30, 00, DateTimeKind.Local)
                                          },
                                          {
                                                  Guid.NewGuid(),
                                                  "30.147.39.139",
                                                  new DateTime(2023, 12, 16, 14, 15, 00, DateTimeKind.Local)
                                          },
                                          {
                                                  Guid.NewGuid(),
                                                  "30.147.39.138",
                                                  new DateTime(2023, 12, 16, 14, 15, 00, DateTimeKind.Local)
                                          },
                                          {
                                                  Guid.NewGuid(),
                                                  "30.147.39.137",
                                                  new DateTime(2023, 12, 16, 14, 15, 00, DateTimeKind.Local)
                                          },
                                          {
                                                  Guid.NewGuid(),
                                                  "202.44.131.225",
                                                  new DateTime(2023, 12, 16, 14, 15, 00, DateTimeKind.Local)
                                          },
                                          {
                                                  Guid.NewGuid(),
                                                  "203.155.34.137",
                                                  new DateTime(2023, 12, 16, 14, 15, 00, DateTimeKind.Local)
                                          },
                                          {
                                                  Guid.NewGuid(),
                                                  "201.100.10.15",
                                                  new DateTime(2023, 12, 16, 14, 15, 00, DateTimeKind.Local)
                                          },
                                          {
                                                  Guid.NewGuid(),
                                                  "4.139.152.132",
                                                  new DateTime(2023, 12, 16, 14, 15, 00, DateTimeKind.Local)
                                          },
                                          {
                                                  Guid.NewGuid(),
                                                  "110.26.122.217",
                                                  new DateTime(2023, 12, 16, 14, 15, 00, DateTimeKind.Local)
                                          },
                                          {
                                                  Guid.NewGuid(),
                                                  "160.103.177.102",
                                                  new DateTime(2023, 12, 16, 14, 15, 00, DateTimeKind.Local)
                                          },
                                          {
                                                  Guid.NewGuid(),
                                                  "163.150.222.228",
                                                  new DateTime(2023, 12, 16, 14, 15, 00, DateTimeKind.Local)
                                          },
                                          {
                                                  Guid.NewGuid(),
                                                  "163.150.222.222",
                                                  new DateTime(2023, 12, 16, 14, 15, 00, DateTimeKind.Local)
                                          },
                                          {
                                                  Guid.NewGuid(),
                                                  "163.150.222.220",
                                                  new DateTime(2023, 12, 16, 14, 15, 00, DateTimeKind.Local)
                                          },
                                          {
                                                  Guid.NewGuid(),
                                                  "195.192.190.170",
                                                  new DateTime(2023, 12, 16, 14, 15, 00, DateTimeKind.Local)
                                          },
                                          {
                                                  Guid.NewGuid(),
                                                  "195.192.190.171",
                                                  new DateTime(2024, 12, 16, 14, 15, 00, DateTimeKind.Local)
                                          },
                                          {
                                                  Guid.NewGuid(),
                                                  "195.192.190.171",
                                                  new DateTime(2024, 12, 16, 15, 15, 00, DateTimeKind.Local)
                                          },
                                          {
                                                  Guid.NewGuid(),
                                                  "195.192.190.171",
                                                  new DateTime(2023, 12, 16, 14, 15, 00, DateTimeKind.Local)
                                          },
                                          {
                                                  Guid.NewGuid(),
                                                  "195.192.190.171",
                                                  new DateTime(2023, 12, 16, 14, 15, 00, DateTimeKind.Local)
                                          },
                                          {
                                                  Guid.NewGuid(),
                                                  "195.192.190.172",
                                                  new DateTime(2023, 12, 16, 14, 15, 00, DateTimeKind.Local)
                                          },
                                          {
                                                  Guid.NewGuid(),
                                                  "195.192.190.174",
                                                  new DateTime(2023, 12, 16, 14, 15, 00, DateTimeKind.Local)
                                          },
                                          {
                                                  Guid.NewGuid(),
                                                  "46.198.29.25",
                                                  new DateTime(2023, 12, 16, 14, 15, 00, DateTimeKind.Local)
                                          },
                                          {
                                                  Guid.NewGuid(),
                                                  "46.198.29.250",
                                                  new DateTime(2023, 12, 16, 14, 15, 00, DateTimeKind.Local)
                                          },
                                          {
                                                  Guid.NewGuid(),
                                                  "46.198.29.5",
                                                  new DateTime(2023, 12, 17, 18, 45, 00, DateTimeKind.Local)
                                          }});

            migrationBuilder.InsertData(table: "Orders",
                                          columns: ["Id", "Number", "Weight", "DistrictId", "DeliveryTime"],
                                          values: new object[,]
                                          {
                                              // Центральный район
                                          {
                                                  Guid.NewGuid(),
                                                  "12345",
                                                  10.5,
                                                  districtIds[0],
                                                  new DateTime(2023, 12, 15, 11, 00, 00, DateTimeKind.Local)
                                          },
                                              // Центральный район
                                          {
                                                  Guid.NewGuid(),
                                                  "12341",
                                                  10.5,
                                                  districtIds[0],
                                                  new DateTime(2023, 12, 15, 11, 30, 00, DateTimeKind.Local)
                                          },
                                              // Центральный район
                                          {
                                                  Guid.NewGuid(),
                                                  "12342",
                                                  10.5,
                                                  districtIds[0],
                                                  new DateTime(2023, 12, 15, 11, 20, 00, DateTimeKind.Local)
                                          },
                                              // Центральный район
                                          {
                                                  Guid.NewGuid(),
                                                  "12343",
                                                  10.5,
                                                  districtIds[0],
                                                  new DateTime(2023, 12, 15, 11, 10, 00, DateTimeKind.Local)
                                          },
                                              // Центральный район
                                          {
                                                  Guid.NewGuid(),
                                                  "12344",
                                                  10.5,
                                                  districtIds[0],
                                                  new DateTime(2023, 12, 15, 11, 40, 00, DateTimeKind.Local)
                                          },
                                              // Центральный район
                                          {
                                                  Guid.NewGuid(),
                                                  "12346",
                                                  10.5,
                                                  districtIds[0],
                                                  new DateTime(2023, 12, 15, 11, 50, 00, DateTimeKind.Local)
                                          },
                                              // Ленинский район
                                          {
                                                  Guid.NewGuid(),
                                                  "67890",
                                                  5.0,
                                                  districtIds[1],
                                                  new DateTime(2023, 12, 16, 12, 30, 00, DateTimeKind.Local)
                                          }, 
                                              // Ленинский район
                                          {
                                                  Guid.NewGuid(),
                                                  "67891",
                                                  5.0,
                                                  districtIds[1],
                                                  new DateTime(2023, 12, 16, 12, 10, 00, DateTimeKind.Local)
                                          }, 
                                              // Ленинский район
                                          {
                                                  Guid.NewGuid(),
                                                  "67892",
                                                  5.0,
                                                  districtIds[1],
                                                  new DateTime(2023, 12, 16, 12, 40, 00, DateTimeKind.Local)
                                          }, 
                                              // Ленинский район
                                          {
                                                  Guid.NewGuid(),
                                                  "67893",
                                                  5.0,
                                                  districtIds[1],
                                                  new DateTime(2023, 12, 16, 12, 20, 00, DateTimeKind.Local)
                                          }, 
                                              // Ленинский район
                                          {
                                                  Guid.NewGuid(),
                                                  "67894",
                                                  5.0,
                                                  districtIds[1],
                                                  new DateTime(2023, 12, 16, 20, 30, 00, DateTimeKind.Local)
                                          }, 
                                              // Ленинский район
                                          {
                                                  Guid.NewGuid(),
                                                  "67895",
                                                  5.0,
                                                  districtIds[1],
                                                  new DateTime(2023, 12, 16, 18, 30, 00, DateTimeKind.Local)
                                          }, 
                                                // Октябрьский район
                                          {
                                                  Guid.NewGuid(),
                                                  "98765",
                                                  2.5,
                                                  districtIds[2],
                                                  new DateTime(2023, 12, 17, 13, 15, 00,
                                                  DateTimeKind.Local)
                                          }, 
                                                // Октябрьский район
                                          {
                                                  Guid.NewGuid(),
                                                  "98761",
                                                  2.5,
                                                  districtIds[2],
                                                  new DateTime(2023, 12, 17, 13, 40, 00,
                                                  DateTimeKind.Local)
                                          }, 
                                                // Октябрьский район
                                          {
                                                  Guid.NewGuid(),
                                                  "98762",
                                                  2.5,
                                                  districtIds[2],
                                                  new DateTime(2023, 12, 17, 13, 20, 00,
                                                  DateTimeKind.Local)
                                          }, 
                                                // Октябрьский район
                                          {
                                                  Guid.NewGuid(),
                                                  "98763",
                                                  2.5,
                                                  districtIds[2],
                                                  new DateTime(2023, 12, 17, 13, 25, 00,
                                                  DateTimeKind.Local)
                                          }, 
                                                // Октябрьский район
                                          {
                                                  Guid.NewGuid(),
                                                  "98764",
                                                  2.5,
                                                  districtIds[2],
                                                  new DateTime(2023, 12, 17, 15, 15, 00,
                                                  DateTimeKind.Local)
                                          }, 
                                                // Октябрьский район
                                          {
                                                  Guid.NewGuid(),
                                                  "98766",
                                                  2.5,
                                                  districtIds[2],
                                                  new DateTime(2023, 12, 20, 13, 15, 00,
                                                  DateTimeKind.Local)
                                          }, 
                                                // Октябрьский район
                                          {
                                                  Guid.NewGuid(),
                                                  "98767",
                                                  2.5,
                                                  districtIds[2],
                                                  new DateTime(2024, 11, 17, 13, 15, 00,
                                                  DateTimeKind.Local)
                                          }
                                          });

            #endregion
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Logbooks");

            migrationBuilder.DropTable(
                name: "Orders");

            migrationBuilder.DropTable(
                name: "Districts");
        }
    }
}
