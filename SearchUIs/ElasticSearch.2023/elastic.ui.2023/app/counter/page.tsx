"use client";

import { useState } from "react";
import { Inter } from "next/font/google";

const inter = Inter({ subsets: ["latin"] });

export default function Counter() {
  const [count, setCount] = useState(0);

  return (
    <div className="flex min-h-screen flex-col items-center justify-between p-24">
      <div className="z-10 w-full max-w-5xl items-center justify-between font-mono text-sm lg:flex">
        <p>You clicked {count} times</p>
        <p>class name {inter.className}</p>
        <button onClick={() => setCount(count + 1)}>Click me</button>
      </div>
    </div>
  );
}
