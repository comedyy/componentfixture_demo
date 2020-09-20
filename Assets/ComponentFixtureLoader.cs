using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComponentFixtureLoader
{
    public void LoadFixture([NotNull] IComponentFixture fixture_script, [NotNull] ComponentFixture fixture_source, int depth)
    {
        //Object[] field_values = fixture_source.FieldValues;

        //foreach (var field_it in Instance._EnumerateField_Verify(fixture_script, fixture_source))
        //{
        //    if (field_it.ArrayLength == 0)
        //    {
        //        this.SetFieldValue(fixture_script, null);
        //    }
        //    else if (field_it.ArrayLength < 0)
        //    {
        //        if (Field.IsFixture)
        //        {
        //            var child_fixture = this.CreateChildFixture();
        //            _PushLoadState();
        //            this.LoadFixture(child_fixture, (ComponentFixture)field_values[field_it.ValueIndex], depth + 1);
        //            _PopLoadState();
        //            this.SetFieldValue(fixture_script, child_fixture);
        //        }
        //        else
        //        {
        //            this.SetFieldValue(fixture_script, field_values[field_it.ValueIndex]);
        //        }
        //    }
        //    else
        //    {
        //        Array array_object;
        //        if (Field.IsFixture)
        //        {
        //            array_object = this.CreateChildFixtures(field_it.ArrayLength);
        //            var new_child_fixtures = new NewChildFixture[field_it.ArrayLength];
        //            _PushLoadState();
        //            int new_count = 0;
        //            for (int i = 0, vi = field_it.ValueIndex; i < field_it.ArrayLength; i++, vi++)
        //            {
        //                var framework_fixture = (ComponentFixture)field_values[vi];
        //                var script_fixture = framework_fixture.FixtureTarget;
        //                if (script_fixture == null)
        //                {
        //                    script_fixture = this.CreateChildFixture();
        //                    new_child_fixtures[new_count++] = new NewChildFixture
        //                    {
        //                        ScriptFixture = script_fixture,
        //                        FrameworkFixture = framework_fixture,
        //                    };
        //                }
        //                array_object.SetValue(script_fixture, i);
        //            }

        //            while (--new_count >= 0)
        //            {
        //                this.LoadFixture(new_child_fixtures[new_count].ScriptFixture, new_child_fixtures[new_count].FrameworkFixture, depth + 1);
        //            }
        //            _PopLoadState();
        //        }
        //        else
        //        {
        //            array_object = this.CreateFieldArray(field_it.ArrayLength);
        //            Array.Copy(field_values, field_it.ValueIndex, array_object, 0, field_it.ArrayLength);
        //        }

        //        this.SetFieldValue(fixture_script, array_object);
        //    }
        //}

        //ComponentFixture.PostLoad(fixture_script, fixture_source, depth);
    }
}
