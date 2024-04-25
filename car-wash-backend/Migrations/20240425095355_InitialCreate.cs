using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace car_wash_backend.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Box",
                columns: table => new
                {
                    box_ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityAlwaysColumn)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Box_pkey", x => x.box_ID);
                });

            migrationBuilder.CreateTable(
                name: "Carwash",
                columns: table => new
                {
                    carwash_ID = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    carwashStreet = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    boxAmount = table.Column<int>(type: "integer", nullable: false),
                    contactInfo = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Carwashes_pkey", x => x.carwash_ID);
                });

            migrationBuilder.CreateTable(
                name: "DayType",
                columns: table => new
                {
                    type_ID = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("DayType_pkey", x => x.type_ID);
                });

            migrationBuilder.CreateTable(
                name: "OrderStatus",
                columns: table => new
                {
                    status_ID = table.Column<Guid>(type: "uuid", nullable: false),
                    statusName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("OrderStatus_pkey", x => x.status_ID);
                });

            migrationBuilder.CreateTable(
                name: "Person",
                columns: table => new
                {
                    person_ID = table.Column<Guid>(type: "uuid", nullable: false),
                    firstName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    lastName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    fathersName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    email = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    phoneNumber = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Person_pkey", x => x.person_ID);
                });

            migrationBuilder.CreateTable(
                name: "Role",
                columns: table => new
                {
                    role_ID = table.Column<Guid>(type: "uuid", nullable: false),
                    roleName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Roles_pkey", x => x.role_ID);
                });

            migrationBuilder.CreateTable(
                name: "ServiceStatus",
                columns: table => new
                {
                    status_ID = table.Column<Guid>(type: "uuid", nullable: false),
                    statusName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("ServiceStatus_pkey", x => x.status_ID);
                });

            migrationBuilder.CreateTable(
                name: "BoxesInCarwash",
                columns: table => new
                {
                    carwash_ID = table.Column<Guid>(type: "uuid", nullable: false),
                    boxesInCarwash_ID = table.Column<Guid>(type: "uuid", nullable: true),
                    box_ID = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.ForeignKey(
                        name: "fk_box_ID",
                        column: x => x.box_ID,
                        principalTable: "Box",
                        principalColumn: "box_ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_carwash_ID",
                        column: x => x.carwash_ID,
                        principalTable: "Carwash",
                        principalColumn: "carwash_ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Day",
                columns: table => new
                {
                    day_ID = table.Column<Guid>(type: "uuid", nullable: false),
                    startTime = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    endTime = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    type_ID = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Day_pkey", x => x.day_ID);
                    table.ForeignKey(
                        name: "fk_type_ID",
                        column: x => x.type_ID,
                        principalTable: "DayType",
                        principalColumn: "type_ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    user_ID = table.Column<Guid>(type: "uuid", nullable: false),
                    login = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    password = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    role_ID = table.Column<Guid>(type: "uuid", nullable: false),
                    person_ID = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Users_pkey", x => x.user_ID);
                    table.ForeignKey(
                        name: "fk_person_ID",
                        column: x => x.person_ID,
                        principalTable: "Person",
                        principalColumn: "person_ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_role_ID",
                        column: x => x.role_ID,
                        principalTable: "Role",
                        principalColumn: "role_ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Service",
                columns: table => new
                {
                    service_ID = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    price = table.Column<long>(type: "bigint", nullable: false),
                    duration = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    status_ID = table.Column<Guid>(type: "uuid", nullable: false),
                    carwash_ID = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Service_pkey", x => x.service_ID);
                    table.ForeignKey(
                        name: "fk_carwash_ID",
                        column: x => x.carwash_ID,
                        principalTable: "Carwash",
                        principalColumn: "carwash_ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_status_ID",
                        column: x => x.status_ID,
                        principalTable: "ServiceStatus",
                        principalColumn: "status_ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Schedule",
                columns: table => new
                {
                    schedule_ID = table.Column<Guid>(type: "uuid", nullable: false),
                    day_ID = table.Column<Guid>(type: "uuid", nullable: false),
                    carwash_ID = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Schedule_pkey", x => x.schedule_ID);
                    table.ForeignKey(
                        name: "fk_day_ID",
                        column: x => x.day_ID,
                        principalTable: "Day",
                        principalColumn: "day_ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Employee",
                columns: table => new
                {
                    employee_ID = table.Column<Guid>(type: "uuid", nullable: false),
                    user_ID = table.Column<Guid>(type: "uuid", nullable: false),
                    carwash_ID = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Employees_pkey", x => x.employee_ID);
                    table.ForeignKey(
                        name: "fk_carwash_ID",
                        column: x => x.carwash_ID,
                        principalTable: "Carwash",
                        principalColumn: "carwash_ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_user_ID",
                        column: x => x.user_ID,
                        principalTable: "User",
                        principalColumn: "user_ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Order",
                columns: table => new
                {
                    order_ID = table.Column<Guid>(type: "uuid", nullable: false),
                    date_time = table.Column<DateOnly>(type: "date", nullable: false),
                    licencePlate = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    box_ID = table.Column<long>(type: "bigint", nullable: false, defaultValueSql: "nextval('\"Orders_box_ID_seq\"'::regclass)"),
                    carwash_ID = table.Column<Guid>(type: "uuid", nullable: false),
                    status_ID = table.Column<Guid>(type: "uuid", nullable: false),
                    user_ID = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Orders_pkey", x => x.order_ID);
                    table.ForeignKey(
                        name: "fk_box_ID",
                        column: x => x.box_ID,
                        principalTable: "Box",
                        principalColumn: "box_ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_carwash_ID",
                        column: x => x.carwash_ID,
                        principalTable: "Carwash",
                        principalColumn: "carwash_ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_status_ID",
                        column: x => x.status_ID,
                        principalTable: "OrderStatus",
                        principalColumn: "status_ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_user_ID",
                        column: x => x.user_ID,
                        principalTable: "User",
                        principalColumn: "user_ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ServicesInOrder",
                columns: table => new
                {
                    service_ID = table.Column<Guid>(type: "uuid", nullable: false),
                    servicesInOrder_ID = table.Column<Guid>(type: "uuid", nullable: true),
                    order_ID = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.ForeignKey(
                        name: "fk_order_ID",
                        column: x => x.order_ID,
                        principalTable: "Order",
                        principalColumn: "order_ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_service_ID",
                        column: x => x.service_ID,
                        principalTable: "Service",
                        principalColumn: "service_ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BoxesInCarwash_carwash_ID",
                table: "BoxesInCarwash",
                column: "carwash_ID");

            migrationBuilder.CreateIndex(
                name: "pk_box_ID",
                table: "BoxesInCarwash",
                column: "box_ID",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Day_type_ID",
                table: "Day",
                column: "type_ID");

            migrationBuilder.CreateIndex(
                name: "IX_Employee_carwash_ID",
                table: "Employee",
                column: "carwash_ID");

            migrationBuilder.CreateIndex(
                name: "IX_Employee_user_ID",
                table: "Employee",
                column: "user_ID");

            migrationBuilder.CreateIndex(
                name: "IX_Order_box_ID",
                table: "Order",
                column: "box_ID");

            migrationBuilder.CreateIndex(
                name: "IX_Order_carwash_ID",
                table: "Order",
                column: "carwash_ID");

            migrationBuilder.CreateIndex(
                name: "IX_Order_status_ID",
                table: "Order",
                column: "status_ID");

            migrationBuilder.CreateIndex(
                name: "IX_Order_user_ID",
                table: "Order",
                column: "user_ID");

            migrationBuilder.CreateIndex(
                name: "IX_Schedule_day_ID",
                table: "Schedule",
                column: "day_ID");

            migrationBuilder.CreateIndex(
                name: "IX_Service_carwash_ID",
                table: "Service",
                column: "carwash_ID");

            migrationBuilder.CreateIndex(
                name: "IX_Service_status_ID",
                table: "Service",
                column: "status_ID");

            migrationBuilder.CreateIndex(
                name: "IX_ServicesInOrder_order_ID",
                table: "ServicesInOrder",
                column: "order_ID");

            migrationBuilder.CreateIndex(
                name: "pk_service_ID",
                table: "ServicesInOrder",
                column: "service_ID",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_User_person_ID",
                table: "User",
                column: "person_ID");

            migrationBuilder.CreateIndex(
                name: "IX_User_role_ID",
                table: "User",
                column: "role_ID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BoxesInCarwash");

            migrationBuilder.DropTable(
                name: "Employee");

            migrationBuilder.DropTable(
                name: "Schedule");

            migrationBuilder.DropTable(
                name: "ServicesInOrder");

            migrationBuilder.DropTable(
                name: "Day");

            migrationBuilder.DropTable(
                name: "Order");

            migrationBuilder.DropTable(
                name: "Service");

            migrationBuilder.DropTable(
                name: "DayType");

            migrationBuilder.DropTable(
                name: "Box");

            migrationBuilder.DropTable(
                name: "OrderStatus");

            migrationBuilder.DropTable(
                name: "User");

            migrationBuilder.DropTable(
                name: "Carwash");

            migrationBuilder.DropTable(
                name: "ServiceStatus");

            migrationBuilder.DropTable(
                name: "Person");

            migrationBuilder.DropTable(
                name: "Role");
        }
    }
}
