import React, { useState } from "react";
import { AppBar, Box, Tab } from "@material-ui/core";
import { TabContext, TabPanel, TabList } from "@material-ui/lab";
import Students from "./Students";

const adminScreenTabs = {
  students: "Students",
  mentors: "Mentors",
};

const AdminScreen = () => {
  const [currentTab, setCurrentTab] = useState(adminScreenTabs.students);
  const handleChange = (e, value) => {
    setCurrentTab(value);
  };

  return (
    <Box marginTop={2}>
      <TabContext value={currentTab}>
        <AppBar position="static">
          <TabList onChange={handleChange}>
            <Tab
              label={adminScreenTabs.students}
              value={adminScreenTabs.students}
            />
            <Tab
              label={adminScreenTabs.mentors}
              value={adminScreenTabs.mentors}
            />
          </TabList>
        </AppBar>
        <TabPanel value={adminScreenTabs.students}>
          <Students />
        </TabPanel>
        <TabPanel value={adminScreenTabs.mentors}></TabPanel>
      </TabContext>
    </Box>
  );
};

export default AdminScreen;
