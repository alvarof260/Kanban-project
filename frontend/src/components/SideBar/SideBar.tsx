import { ProfileSummary, NavBar, UserActions } from "./components";

export const SideBar = () => {
  return (
    <aside className='py-6 px-2 h-[calc(100vh-48px)] w-60 flex flex-col justify-start gap-6 bg-bg-100'>

      <ProfileSummary />

      <NavBar />

      <UserActions />

    </aside>
  );
};
