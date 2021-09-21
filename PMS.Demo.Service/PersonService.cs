using PMS.Demo.Entity;
using PMS.Demo.Runtime;
using PMS.Demo.Service.Interface;
using PMS.Demo.Utils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace PMS.Demo.Service
{
    public class PersonService : IEntity<Person>
    {

        private string tablename = "t_person";
        //Person CRUD
        public int Delete(string id)
        {
            return Global.dbhelper.ExecuteNonQuery($"DELETE FROM {tablename} WHERE id = '{id}'");
        }

        public int Insert(Person t)
        {
            return Global.dbhelper.ExecuteNonQuery($"INSERT INTO 'main'.'{tablename}' ('id', 'name', 'gender', 'age', 'photopath', 'birthday') VALUES " + $"('{t.id}', '{t.name}', '{t.gender}', {t.age}, '{t.photopath}', '{t.birthday}')");
        }

        public Person Query(string id)
        {
            var table = Global.dbhelper.Fill($"SELECT * FROM {tablename} WHERE id = '{id}'");
            if (table == null || table.Rows.Count == 0)
                return null;
            return new Person() { age = Convert.ToInt32(table.Rows[0].Field<object>("age")), birthday = table.Rows[0].Field<string>("birthday"), gender = table.Rows[0].Field<string>("gender"), id = table.Rows[0].Field<string>("id"), name = table.Rows[0].Field<string>("name"), photopath = table.Rows[0].Field<string>("photopath") };
        }

        public DataTable QueryList()
        {
            return Global.dbhelper.Fill( $"SELECT * FROM {tablename}");
        }

        public int Update(Person t)
        {
            return Global.dbhelper.ExecuteNonQuery($"UPDATE 'main'.'{tablename}' SET 'name' = '{t.name}', 'gender' = '{t.gender}', 'age' = {t.age}, 'photopath' = '{t.photopath}', 'birthday' = '{t.birthday}' WHERE id = '{t.id}'");
        }

        
    }
}
