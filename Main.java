import com.formdev.flatlaf.FlatDarkLaf;

import javax.swing.*;
import javax.swing.filechooser.FileNameExtensionFilter;
import java.awt.*;
import java.awt.event.ActionEvent;
import java.awt.event.ActionListener;
import java.awt.event.ItemEvent;
import java.awt.event.ItemListener;
import java.io.*;
import java.util.Objects;

public class Main extends JFrame implements ActionListener {

    private final JTextArea textArea;
    private boolean isWrapLine = true;
    private EncryptString es;

    public Main() {
        try {
            es = new EncryptString();
        } catch (Exception e) {
            e.printStackTrace();
        }

        setTitle("YuXiang CipherPad");
        setSize(800, 500);
        setIconImage(new ImageIcon(Objects.requireNonNull(this.getClass().getResource("/icon/icon.png"))).getImage());
        setLocationRelativeTo(null);
        setDefaultCloseOperation(JFrame.EXIT_ON_CLOSE);

        JMenuBar menuBar = new JMenuBar();

        JMenu fileMenu = new JMenu("文件");
        JMenuItem openMenuItem = new JMenuItem("打开");
        JMenuItem saveMenuItem = new JMenuItem("保存");
        openMenuItem.addActionListener(this);
        saveMenuItem.addActionListener(this);
        fileMenu.add(openMenuItem);
        fileMenu.add(saveMenuItem);

        JMenu editMenu = new JMenu("编辑");
        JMenuItem clearMenuItem = new JMenuItem("清空");
        clearMenuItem.addActionListener(this);
        editMenu.add(clearMenuItem);

        JMenu settingMenu = new JMenu("设置");
        JCheckBoxMenuItem customKeyMenuItem = new JCheckBoxMenuItem("密钥");
        JCheckBoxMenuItem setWrapLineMenuItem = new JCheckBoxMenuItem("换行", true);
        customKeyMenuItem.addItemListener(e -> {
            if (customKeyMenuItem.getState()) {
                String key = JOptionPane.showInputDialog(Main.this, "请输入你自己的密钥：", "使用自定义密钥", JOptionPane.PLAIN_MESSAGE);
                if (key != null) {
                    if (key.length() == 16 || key.length() == 24 || key.length() == 32) {
                        try {
                            es = new EncryptString(key);
                        } catch (Exception ex) {
                            ex.printStackTrace();
                        }
                    } else {
                        customKeyMenuItem.setState(false);
                        JOptionPane.showMessageDialog(Main.this, "无效的AES密钥长度。\n请使用16字节、24字节或32字节的密钥长度。", "错误", JOptionPane.ERROR_MESSAGE);
                    }
                } else {
                    customKeyMenuItem.setState(false);
                }
            } else {
                try {
                    es = new EncryptString();
                } catch (Exception ex) {
                    ex.printStackTrace();
                }
            }
        });
        setWrapLineMenuItem.addItemListener(new ItemListener() {
            @Override
            public void itemStateChanged(ItemEvent e) {
                isWrapLine = !isWrapLine;
                if (isWrapLine) {
                    textArea.setLineWrap(true);
                    textArea.setWrapStyleWord(true);
                } else {
                    textArea.setLineWrap(false);
                    textArea.setWrapStyleWord(false);
                }
            }
        });
        settingMenu.add(customKeyMenuItem);
        settingMenu.add(setWrapLineMenuItem);

        menuBar.add(fileMenu);
        menuBar.add(editMenu);
        menuBar.add(settingMenu);
        setJMenuBar(menuBar);

        textArea = new JTextArea();
        textArea.setLineWrap(true);
        textArea.setWrapStyleWord(true);
        add(new JScrollPane(textArea), BorderLayout.CENTER);

        setVisible(true);
    }

    public static void main(String[] args) {
        FlatDarkLaf.setup();
        new Main();
    }

    @Override
    public void actionPerformed(ActionEvent e) {
        String command = e.getActionCommand();
        switch (command) {
            case "打开":
                openFile();
                break;
            case "保存":
                saveFile();
                break;
            case "清空":
                int isClear = JOptionPane.showConfirmDialog(Main.this, "是否清空？", "清空", JOptionPane.YES_NO_OPTION, JOptionPane.QUESTION_MESSAGE);
                if (isClear == 0) {
                    textArea.setText("");
                }
                break;
        }
    }

    private void openFile() {
        JFileChooser fileChooser = new JFileChooser();
        fileChooser.setFileSelectionMode(JFileChooser.FILES_ONLY);
        fileChooser.setFileFilter(new FileNameExtensionFilter("加密文档(*.es)", "es"));
        int result = fileChooser.showOpenDialog(this);
        if (result == JFileChooser.APPROVE_OPTION) {
            File file = fileChooser.getSelectedFile();
            try (BufferedReader reader = new BufferedReader(new FileReader(file))) {
                textArea.read(reader, null);
                textArea.setText(es.decrypt(textArea.getText()));
            } catch (Exception e) {
                JOptionPane.showMessageDialog(Main.this, "文件读取错误，请检查其是否为本程序加密的文档。\n如果是，请检查密钥是否与源文件所使用的密钥匹配。", "错误", JOptionPane.ERROR_MESSAGE);
                textArea.setText("");
            }
        }
    }

    private void saveFile() {
        JFileChooser fileChooser = new JFileChooser();
        fileChooser.setFileSelectionMode(JFileChooser.FILES_ONLY);
        fileChooser.setFileFilter(new FileNameExtensionFilter("加密文档(*.es)", "es"));
        int result = fileChooser.showSaveDialog(this);
        if (result == JFileChooser.APPROVE_OPTION) {
            File file = fileChooser.getSelectedFile();
            try (BufferedWriter writer = new BufferedWriter(new FileWriter(file))) {
                textArea.setText(es.encrypt(textArea.getText()));
                textArea.write(writer);
            } catch (Exception e) {
                e.printStackTrace();
            }
        }
        textArea.setText("");
    }
}
